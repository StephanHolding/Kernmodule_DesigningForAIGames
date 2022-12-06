using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPlace : MonoBehaviour, IFindable
{

    private const float HIDING_AREA = 0.5f;
    

    private Vector3 GetHidingPosition()
    {
        return transform.position;
    }

    private bool IsTaken(Transform _agentPosition)
    {
        return Vector3.Distance(_agentPosition.position, transform.position) <= HIDING_AREA;
    }

    public bool FindingCondition(Transform _searcher)
    {
        return !IsTaken(_searcher);
    }

    public Vector3 GetMoveToPosition()
    {
        return GetHidingPosition();
    }
}
