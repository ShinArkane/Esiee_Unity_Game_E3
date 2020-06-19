using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FollowPathEnnemy : MonoBehaviour
{
    public enum moveType { UseTransform };
    public moveType moveTypes;
    public Transform[] pathPoints;
    public int CurrentPath;
    private Rigidbody rb;
    public float reachDistance = 5.0f;
    public float speed = 5.0f;

    private void FixedUpdate()
    {
        switch (moveTypes)
        {
            case moveType.UseTransform:
                UseTransform();
                break;
        }
    }

    void UseTransform()
    {
        Vector3 dir = pathPoints[CurrentPath].position - transform.position;
        Vector3 dirNorm = dir.normalized;
        transform.Translate(dirNorm * (speed*Time.fixedDeltaTime));

        if(dir.magnitude<=reachDistance)
        {
            CurrentPath++;
            if(CurrentPath >= pathPoints.Length)
            {
                CurrentPath = 0;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (pathPoints.Length == null)
            return;
        foreach (Transform pathPoint in pathPoints)
        {
            if (pathPoint)
            {
                Gizmos.DrawSphere(pathPoint.position, reachDistance);
            }
        }
    }
}
