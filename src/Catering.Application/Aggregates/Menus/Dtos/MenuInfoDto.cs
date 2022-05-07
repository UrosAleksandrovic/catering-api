using Catering.Application.Aggregates.Menus.Dtos;

namespace Catering.Application.Dtos.Menu;

public class MenuInfoDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public MenuContactInfoDto Contact { get; set; }
}
