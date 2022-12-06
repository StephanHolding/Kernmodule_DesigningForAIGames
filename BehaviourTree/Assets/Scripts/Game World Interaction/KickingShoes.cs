using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class KickingShoes : Pickupable, IFindable
{

    private GameObject leftShoe;
    private GameObject rightShoe;

    private Vector3 originalLocalPositionLeft;
    private Vector3 originalLocalPositionRight;
    
    private void Awake()
    {
        leftShoe = transform.GetChild(0).gameObject;
        rightShoe = transform.GetChild(1).gameObject;

        originalLocalPositionLeft = leftShoe.transform.localPosition;
        originalLocalPositionRight = rightShoe.transform.localPosition;
    }

    public override void Pickup(PickupController _controller)
    {
        leftShoe.transform.SetParent(_controller.leftFoot.transform);
        rightShoe.transform.SetParent(_controller.rightFoot.transform);

        leftShoe.transform.localPosition = Vector3.zero;
        leftShoe.transform.localEulerAngles = Vector3.zero;
        rightShoe.transform.localPosition = Vector3.zero;
        rightShoe.transform.localEulerAngles = Vector3.zero;
    }

    public override void Drop(PickupController _controller)
    {
        leftShoe.transform.SetParent(transform);
        leftShoe.transform.localPosition = originalLocalPositionLeft;
        rightShoe.transform.SetParent(transform);
        rightShoe.transform.localPosition = originalLocalPositionRight;
    }

    public bool FindingCondition(Transform _searcher)
    {
        return true;
    }

    public Vector3 GetMoveToPosition()
    {
        return transform.position;
    }
}
