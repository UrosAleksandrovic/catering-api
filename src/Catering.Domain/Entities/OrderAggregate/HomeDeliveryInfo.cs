using Ardalis.GuardClauses;

namespace Catering.Domain.Entities.OrderAggregate;

public class HomeDeliveryInfo
{
    public string StreetAndHouse { get; private set; }
    public string FloorAndAppartment { get; private set; }

    private HomeDeliveryInfo() { }

    public HomeDeliveryInfo(string streetAndHouse, string floorAndAppartment)
    {
        Guard.Against.NullOrWhiteSpace(streetAndHouse);

        StreetAndHouse = streetAndHouse;
        FloorAndAppartment = floorAndAppartment;
    }
}
