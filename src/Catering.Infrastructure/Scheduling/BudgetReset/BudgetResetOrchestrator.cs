using Catering.Application.Scheduling;
using Catering.Application.Scheduling.BudgetReset;
using Catering.Application.Scheduling.BudgetReset.Notifications;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Fallback;
using Polly.Retry;

namespace Catering.Infrastructure.Scheduling.BudgetReset
{
    internal class BudgetResetOrchestrator : BackgroundService, IJobOrchestrator
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<BudgetResetOrchestrator> logger;
        private readonly BudetResetJobSettings settings;

        private bool isCalibrated = false;
        private PeriodicTimer timer;

        private static readonly TimeSpan oneDaySpan = TimeSpan.FromDays(1);

        public BudgetResetOrchestrator(
            ILogger<BudgetResetOrchestrator> logger,
            IOptions<BudetResetJobSettings> options,
            IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger;
            this.settings = options.Value;
            this.serviceScopeFactory = serviceScopeFactory;

            CalibrateTimer();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                if (!isCalibrated)
                {
                    CalibrateTimer();
                }

                await RunAllJobsAsync();
            }
        }

        private void CalibrateTimer()
        {
            var timeZones = TimeZoneInfo.FindSystemTimeZoneById(settings.TimeZoneOfExecutionIana);
            var currentTime = DateTimeOffset.UtcNow.ToOffset(timeZones.BaseUtcOffset);

            if (currentTime.TimeOfDay < settings.ExecutionStartTime
                || currentTime.TimeOfDay > settings.ExecutionEndTime)
            {
                RecalibrateForTimeOfExecution(currentTime);
                return;
            }

            var nextExecutionDate = GetNextExecutionDate(currentTime, timeZones);
            timer = new PeriodicTimer(nextExecutionDate.Subtract(currentTime));
            logger.LogInformation(
                "Budget Reset Orchestrator: Recalibrated at {currentTime}, next execution is scheduled for {nextExecution}",
                currentTime,
                nextExecutionDate.ToString());

            isCalibrated = true;
        }

        private void RecalibrateForTimeOfExecution(DateTimeOffset currentTime)
        {
            var timeToWait = oneDaySpan.TotalMinutes - currentTime.TimeOfDay.TotalMinutes + 1;
            timer = new PeriodicTimer(TimeSpan.FromMinutes(timeToWait));
            isCalibrated = false;

            logger.LogInformation(
                "Budget Reset Orchestrator: Waiting {timeToWait} minutes to execution time to recalibrate.",
                Math.Ceiling(timeToWait));
        }

        private DateTimeOffset GetNextExecutionDate(DateTimeOffset currentTime, TimeZoneInfo timeZone)
            => new(
                year: currentTime.Year,
                month: currentTime.Month + 1,
                day: 1,
                hour: settings.ExecutionStartTime.Hours,
                minute: settings.ExecutionStartTime.Minutes,
                second: settings.ExecutionStartTime.Seconds,
                offset: timeZone.BaseUtcOffset);

        public async Task RunAllJobsAsync()
        {
            using var serviceScope = serviceScopeFactory.CreateScope();

            var budgetResetJobs = serviceScope.ServiceProvider.GetServices<IBudgetResetJob>().OrderBy(x => x.Priority);
            var publisher = serviceScope.ServiceProvider.GetRequiredService<IMediator>();

            foreach (var job in budgetResetJobs)
            {
                var retryPolicy = GetPollyResiliancePipeline(job.GetName(), publisher);

                await retryPolicy.ExecuteAsync(async token => 
                {
                    if (token.IsCancellationRequested)
                        return true;

                    return await job.ExecuteAsync();
                });
            }
        }

        private ResiliencePipeline<bool> GetPollyResiliancePipeline(string jobName, IMediator publisher)
        {
            var retryOptions = new RetryStrategyOptions<bool>
            {
                MaxRetryAttempts = 3,
                ShouldHandle = new PredicateBuilder<bool>().Handle<Exception>().HandleResult(result => !result),
                DelayGenerator = args => new ValueTask<TimeSpan?>(TimeSpan.FromSeconds(args.AttemptNumber * 10)),
                OnRetry = args =>
                {
                    if (args.Outcome.Exception != null)
                    {
                        logger.LogError(args.Outcome.Exception,
                            "Budget Reset Orchestrator: Failed to execute report \"{reportName}\"",
                            jobName);
                    }

                    return ValueTask.CompletedTask;
                }
            };

            var fallbackOptions = new FallbackStrategyOptions<bool>
            {
                ShouldHandle = new PredicateBuilder<bool>().Handle<Exception>().HandleResult(result => !result),
                FallbackAction = args =>
                {
                    _ = publisher.Publish(new BudgetResetFailed { Month = DateTimeOffset.UtcNow.Month });
                    return Outcome.FromResultAsValueTask(true);
                }
            };

            return new ResiliencePipelineBuilder<bool>()
                .AddRetry(retryOptions)
                .AddFallback(fallbackOptions)
                .Build();
        }
    }
}
