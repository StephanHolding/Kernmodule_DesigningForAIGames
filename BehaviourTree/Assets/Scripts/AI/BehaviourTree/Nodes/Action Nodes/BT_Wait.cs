using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Wait : BTBaseNode
{
    private readonly float waitTime;
    private float timeLeft;
    
    public BT_Wait(float _waitTime)
    {
        waitTime = _waitTime;
        timeLeft = _waitTime;
    }
    
    public override TaskStatus Run()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft > 0)
        {
            return TaskStatus.Running;
        }

        timeLeft = waitTime;
        return TaskStatus.Success;
    }
}
