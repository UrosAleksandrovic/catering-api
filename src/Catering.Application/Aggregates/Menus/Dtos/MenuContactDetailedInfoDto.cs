using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Application.Aggregates.Menus.Dtos;

public class MenuContactDetailedInfoDto
{
    public Guid MenuId { get; set; }
    public string MenuName { get; set; }
    public string IdentityId { get; set; }
    public string Email { get; set; }
    public FullName FullName { get; set; }
}
