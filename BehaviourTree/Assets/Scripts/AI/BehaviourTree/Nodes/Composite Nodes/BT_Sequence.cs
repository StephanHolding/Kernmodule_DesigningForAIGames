
using UnityEngine;

public class BT_Sequence : BTBaseNode
{

    private readonly BTBaseNode[] childrenNodes;
    private int childIndex = 0;
    private readonly ConditionFunction successCondition;

    public BT_Sequence(BTBaseNode[] _children)
    {
        childrenNodes = _children;
    }

    public BT_Sequence( ConditionFunction _successCondition, BTBaseNode[] _children)
    {
        childrenNodes = _children;
        successCondition = _successCondition;
    }
    
    public override TaskStatus Run()
    {
        if (successCondition != null)
        {
            if (!successCondition.Invoke())
            {
                foreach (BTBaseNode node in childrenNodes)
                {
                    node.Reset();
                }
                Reset();
                return TaskStatus.Failed;
            }
        }
        
        for (; childIndex < childrenNodes.Length; childIndex++)
        {
            BTBaseNode toRun = childrenNodes[childIndex];
            TaskStatus childReturnStatus = toRun.Run();

            switch (childReturnStatus)
            {
                case TaskStatus.Failed:
                    childIndex = 0;
                    return TaskStatus.Failed;
                case TaskStatus.Running:
                    return TaskStatus.Running;
                case TaskStatus.Success:
                    continue;
            }
        }
        
        childIndex = 0;
        return TaskStatus.Success;
    }

    public override void Reset()
    {
        childIndex = 0;
    }
}
