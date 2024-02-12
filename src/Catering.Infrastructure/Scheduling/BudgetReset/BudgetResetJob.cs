using System.Net.Http.Json;
using Catering.Application.Scheduling;
using Catering.Application.Scheduling.BudgetReset;
using Catering.Application.Scheduling.BudgetReset.Requests;
using Catering.Infrastructure.Scheduling.BudgetReset.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Catering.Infrastructure.Scheduling.BudgetReset
{
    internal class BudgetResetJob : IBudgetResetJob
    {
        private readonly ILogger<BudgetResetJob> _logger;
        private readonly BudetResetJobSettings _settings;
        private readonly IJobLogRepository _logRepository;
        private readonly IMediator _publisher;

        public BudgetResetJob(
            ILogger<BudgetResetJob> logger,
            IOptions<BudetResetJobSettings> settings,
            IJobLogRepository logRepository,
            IMediator publisher)
        {
            _logger = logger;
            _settings = settings.Value;
            _logRepository = logRepository;
            _publisher = publisher;
        }

        public int Priority => 100;

        public async Task<bool> ExecuteAsync()
        {
            var reportName = GetName();
            var jobLog = await _logRepository.CreateAsync(reportName);

            try
            {
                var newBudget = await GetNewBudgetAsync();

                await _publisher.Send(new ResetCustomerBudgets { NewBudget = newBudget});
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{reportName}: Failed to reset the budget.", reportName);
                return false;
            }

            jobLog.IsSuccessful = true;
            jobLog.ExecutedAt = DateTimeOffset.UtcNow;
            await _logRepository.UpdateAsync(jobLog);

            return true;
        }

        public string GetName() => "Budget Reset";

        private async Task<double> GetNewBudgetAsync()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetFromJsonAsync<CurrencyRatesDto>(_settings.CurrencyRateUrl);

            return _settings.DefaultValueEur * Math.Ceiling(response.ExchangeMiddle);
        }
    }
}
