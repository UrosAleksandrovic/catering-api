namespace Catering.Application.Aggregates.Expenses.Dtos
{
    public class CreateExpenseDto
    {
        public Guid MenuId { get; set; }
        public string CustomerId { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }
        public DateTimeOffset DeliveredOn { get; set; }
    }
}
