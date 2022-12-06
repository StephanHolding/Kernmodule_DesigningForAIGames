using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_SetState : BTBaseNode
{
    private readonly IHasState agent;
    private readonly int newState;
    
    public BT_SetState(IHasState _agent, int _newState)
    {
        agent = _agent;
        newState = _newState;
    }
    
    public override TaskStatus Run()
    {
        agent.SetState(newState);
        return TaskStatus.Success;
    }
}
