using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_FindClosest<T> : BTBaseNode where T : MonoBehaviour, IFindable
{
    private readonly Transform agent;
    private readonly Blackboard blackboard;
    private readonly string positionKey;

    public BT_FindClosest(Transform _agent, Blackboard _blackboard, string _positionKey)
    {
        agent = _agent;
        blackboard = _blackboard;
        positionKey = _positionKey;
    }

    public override TaskStatus Run()
    {
        T[] allObjectsOfT = Object.FindObjectsOfType<T>();
        Print(allObjectsOfT.Length.ToString());
        T closestT = FindClosestT(allObjectsOfT);
        blackboard.SetVariable(positionKey, closestT.GetMoveToPosition());
        return TaskStatus.Success;
    }

    private T FindClosestT(T[] _allT)
    {
        T toReturn = null;
        float closest = Mathf.Infinity;

        for (int i = 0; i < _allT.Length; i++)
        {
            T toCheck = _allT[i];
            float distance = Vector3.Distance(agent.position, toCheck.transform.position);

            if (toCheck.FindingCondition(agent) && distance < closest)
            {
                toReturn = toCheck;
                closest = distance;
            }
        }

        return toReturn;
    }
}