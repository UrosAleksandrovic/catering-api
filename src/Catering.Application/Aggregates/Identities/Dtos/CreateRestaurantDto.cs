namespace Catering.Application.Aggregates.Identities.Dtos;

public class CreateRestaurantDto
{
    public string Email { get; set; }
    public string InitialPassword { get; set; }
    public string Name { get; set; }
}
