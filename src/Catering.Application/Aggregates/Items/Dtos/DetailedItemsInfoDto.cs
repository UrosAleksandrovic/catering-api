namespace Catering.Application.Aggregates.Items.Dtos;

public class DetailedItemsInfoDto : ItemInfoDto
{
    public IEnumerable<string> Categories { get; set; }
    public IEnumerable<string> Ingredients { get; set; }
}
