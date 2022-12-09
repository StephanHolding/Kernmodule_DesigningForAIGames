
public class BT_Condition : BTBaseNode
{
    private readonly ConditionFunction conditionToCheck;
    
    public BT_Condition(ConditionFunction _conditionFunction)
    {
        conditionToCheck = _conditionFunction;
    }

    public override TaskStatus Run()
    {
        if (conditionToCheck.Invoke())
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failed;
    }
}
