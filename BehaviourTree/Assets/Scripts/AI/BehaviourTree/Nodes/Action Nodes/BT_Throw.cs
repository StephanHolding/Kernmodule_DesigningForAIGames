using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Throw : BTBaseNode
{
    private readonly Guard guard;
    
    public BT_Throw(Guard _guard)
    {
        guard = _guard;
    }
    
    public override TaskStatus Run()
    {
        guard.GetSmokeBombed();
        return TaskStatus.Success;
    }
}
