using UnityEngine;
using UnityEngine.AI;

public class BT_MoveTowards : BTBaseNode
{
    private readonly string blackboardKey;
    private readonly NavMeshAgent agent;
    private readonly Blackboard blackboard;
    private bool destinationWasSet;
    private readonly bool useBlackboard;
    private readonly Transform target;

    public BT_MoveTowards(NavMeshAgent _agent, Blackboard _blackboard, string _blackboardKey)
    {
        //For a static position
        
        agent = _agent;
        blackboardKey = _blackboardKey;
        blackboard = _blackboard;
        useBlackboard = true;
    }

    public BT_MoveTowards(NavMeshAgent _agent, Transform _target)
    {
        //for a dynamic position
        
        agent = _agent;
        target = _target;
        useBlackboard = false;
    }

    public override TaskStatus Run()
    {
        if (!destinationWasSet)
        {
            Vector3 targetPosition = useBlackboard ? blackboard.GetVariable<Vector3>(blackboardKey) : target.position;

            agent.SetDestination(targetPosition);
            Print(agent.gameObject.name + " is moving towards " + targetPosition);
            destinationWasSet = true;
            return TaskStatus.Running;
        }

        if (agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            destinationWasSet = false;
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

    public override void Reset()
    {
        agent.ResetPath();
        destinationWasSet = false;
    }
}