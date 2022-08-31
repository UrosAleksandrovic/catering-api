namespace Catering.Application.Aggregates.Items.Dtos;

public class UpdateItemDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    public string[] Ingredients { get; set; }
    public string[] Categories { get; set; }
}
