using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_GetNextPatrolWaypoint : BTBaseNode
{
    private int waypointIndex;
    private readonly Transform[] allWaypoints;
    private readonly Blackboard blackboard;
    private readonly string blackboardKey;


    public BT_GetNextPatrolWaypoint(Transform[] _allWaypoints, Blackboard _blackboard, string _blackboardKey)
    {
        waypointIndex = 0;
        allWaypoints = _allWaypoints;
        blackboard = _blackboard;
        blackboardKey = _blackboardKey;
    }

    public override TaskStatus Run()
    {
        if (waypointIndex + 1 < allWaypoints.Length)
            waypointIndex++;
        else
            waypointIndex = 0;
        
        blackboard.SetVariable(blackboardKey, allWaypoints[waypointIndex].position);
        return TaskStatus.Success;
    }

    public override void Reset()
    {
        waypointIndex = 0;
    }
}