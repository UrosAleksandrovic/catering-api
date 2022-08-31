namespace Catering.Application.Aggregates.Items.Dtos;

public class ItemInfoDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double TotalRating { get; set; }
    public decimal Price { get; set; }
    public Guid MenuId { get; set; }

    public IEnumerable<string> Categories { get; set; }
    public IEnumerable<string> Ingredients { get; set; }
}
