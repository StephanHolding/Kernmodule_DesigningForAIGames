public interface IAbleToPickup
{
    void Pickup(Pickupable _toPickup);
    void Drop();
    bool CanPickup(Pickupable _toPickup);
}
