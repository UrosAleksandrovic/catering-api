using Catering.Application.Scheduling;
using Catering.Application.Scheduling.BudgetReset;
using Catering.Infrastructure.Scheduling;
using Catering.Infrastructure.Scheduling.BudgetReset;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catering.DependencyInjection
{
    public static class SchedulingExtensions
    {
        public static IServiceCollection AddJobScheduling(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var settingsSection = configuration.GetSection(BudetResetJobSettings.Position);

            services.AddOptions<BudetResetJobSettings>()
                .Bind(settingsSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddTransient<IBudgetResetJob, BudgetResetJob>();
            services.AddHostedService<BudgetResetOrchestrator>();
            services.AddTransient<IJobLogRepository, JobLogRepository>();

            return services;
        }
    }
}
