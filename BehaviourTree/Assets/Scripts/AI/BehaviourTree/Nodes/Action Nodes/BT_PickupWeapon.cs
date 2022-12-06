using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_PickupWeapon : BTBaseNode
{
    public delegate Pickupable PickupableReference();
    
    private readonly PickupController agent;
    private readonly PickupableReference weaponInWorld;
    
    public BT_PickupWeapon(PickupableReference _weaponInWorld, PickupController _agent)
    {
        weaponInWorld = _weaponInWorld;
        agent = _agent;
    }
    
    public override TaskStatus Run()
    {
        if (agent.CanPickup(weaponInWorld.Invoke()))
            return TaskStatus.Failed;
        
        agent.Pickup(weaponInWorld.Invoke());
        return TaskStatus.Success;
    }

}
