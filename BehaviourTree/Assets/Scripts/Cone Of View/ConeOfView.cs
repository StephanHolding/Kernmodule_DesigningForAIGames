using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConeOfView : MonoBehaviour
{
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public float viewRange;
    public int viewAngle;
    private void OnDrawGizmos()
    {
        Handles.color = Color.white;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, viewRange);

        Vector3 lineDirection1 = DirectionFromAngle(transform.eulerAngles.y, -viewAngle / 2f);
        Vector3 lineDirection2 = DirectionFromAngle(transform.eulerAngles.y, viewAngle / 2f);

        Handles.color = Color.magenta;
        Handles.DrawLine(transform.position, transform.position + lineDirection1 * viewRange);
        Handles.DrawLine(transform.position, transform.position + lineDirection2 * viewRange);
    }

    public bool CheckConeOfView()
    {
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, viewRange, targetMask);

        if (collidersInRange.Length > 0)
        {
            for (int i = 0; i < collidersInRange.Length; i++)
            {
                Transform toCheck = collidersInRange[i].transform;
                Vector3 directionOfObject = (toCheck.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionOfObject) < viewAngle / 2f)
                {
                    if (!Physics.Raycast(transform.position, directionOfObject, viewRange, obstructionMask))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private Vector3 DirectionFromAngle(float _eulerY, float _angleInDegrees)
    {
        _angleInDegrees += _eulerY;
        return new Vector3(Mathf.Sin(_angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(_angleInDegrees * Mathf.Deg2Rad));
    }
}