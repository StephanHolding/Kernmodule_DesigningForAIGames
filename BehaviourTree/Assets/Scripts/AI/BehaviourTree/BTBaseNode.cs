using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskStatus
{
    Success,
    Failed,
    Running
}

public abstract class BTBaseNode
{

    public delegate bool ConditionFunction();

    public abstract TaskStatus Run();

    public virtual void Reset()
    {
        
    }

    protected void Print(string _message)
    {
        Debug.Log(_message);
    }
}