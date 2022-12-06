using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickupable : MonoBehaviour
{

    public abstract void Pickup(PickupController _controller);
    public abstract void Drop(PickupController _controller);

}
