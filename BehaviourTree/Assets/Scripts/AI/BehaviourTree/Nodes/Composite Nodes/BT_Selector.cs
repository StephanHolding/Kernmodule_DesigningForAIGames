using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Selector : BTBaseNode
{

    private readonly BTBaseNode[] children;
    private int childIndex;
    
    public BT_Selector(BTBaseNode[] _children)
    {
        this.children = _children;
    }

    public override TaskStatus Run()
    {

        for (; childIndex < children.Length; childIndex++)
        {
            TaskStatus returnStatus = children[childIndex].Run();
            switch (returnStatus)
            {
                case TaskStatus.Failed:
                    continue;
                case TaskStatus.Running:
                    return TaskStatus.Running;
                case TaskStatus.Success:
                    childIndex = 0;
                    return TaskStatus.Success;
            }
        }

        childIndex = 0;
        return TaskStatus.Failed;
    }
}
