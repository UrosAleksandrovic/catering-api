namespace Catering.Application.Aggregates.Expenses.Dtos
{
    public class UpdateExpenseDto
    {
        public string Note { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset DeliveredOn { get; set; }
    }
}
