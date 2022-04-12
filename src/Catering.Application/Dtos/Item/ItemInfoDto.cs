namespace Catering.Application.Dtos.Item;

public class ItemInfoDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double TotalRating { get; set; }
    public Guid MenuId { get; set; }
}
