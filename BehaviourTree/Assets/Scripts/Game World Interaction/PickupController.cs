using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    [Header("Attachment Objects")] 
    public GameObject rightHand;
    public GameObject leftHand;
    public GameObject rightFoot;
    public GameObject leftFoot;
    
    
    protected Pickupable holdingObject;
    protected Pickupable pickupableInRange;
    
    protected const float PICKUP_RANGE = 0.1f;
    
    protected  virtual void OnTriggerEnter(Collider _other)
    {
        Pickupable p = _other.GetComponent<Pickupable>();
        if (p != null && pickupableInRange == null)
        {
            pickupableInRange = p;
            return;
        }
    }

    protected  virtual void OnTriggerExit(Collider _other)
    {
        Pickupable p = _other.GetComponent<Pickupable>();
        if (p != null && p == pickupableInRange)
        {
            pickupableInRange = null;
            return;
        }
    }

    
    public void Pickup(Pickupable _toPickup)
    {
        _toPickup.Pickup(this);
        holdingObject = _toPickup;
    }

    public void Drop()
    {
        holdingObject.Drop(this);
    }

    public bool CanPickup(Pickupable _toPickup)
    {
        return holdingObject == null &&
               Vector3.Distance(transform.position, _toPickup.transform.position) <= PICKUP_RANGE;
    }
}
