using System.Text.Json.Serialization;

namespace Catering.Infrastructure.Scheduling.BudgetReset.Dtos
{
    internal class CurrencyRatesDto
    {
        public string Code { get; set; }
        public DateTime Date { get; set; }

        [JsonPropertyName("exchange_middle")]
        public double ExchangeMiddle { get; set; }
    }
}
