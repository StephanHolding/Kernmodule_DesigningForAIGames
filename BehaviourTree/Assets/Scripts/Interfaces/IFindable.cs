using UnityEngine;

public interface IFindable
{
    bool FindingCondition(Transform _searcher);
    Vector3 GetMoveToPosition();
}