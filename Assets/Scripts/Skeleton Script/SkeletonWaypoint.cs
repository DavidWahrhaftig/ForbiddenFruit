using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;

public class SkeletonWaypoint : MonoBehaviour
{
    [SerializeField]
    protected float debugDrawRadius = 1.0f;

    [SerializeField]
    protected float proximityRadius = 50f;

    List<SkeletonWaypoint> neighbours;

    public void Start()
    {
        // Find all skeleton waypoints in the scene
        GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("SkeletonWaypoint");

        neighbours = new List<SkeletonWaypoint>();

        // Check if waypoints are close enough
        for (int w = 0; w < allWaypoints.Length; w++)
        {
            SkeletonWaypoint nextWaypoint = allWaypoints[w].GetComponent<SkeletonWaypoint>();

            if(nextWaypoint != null)
            {
                if(Vector3.Distance(this.transform.position, nextWaypoint.transform.position) <= proximityRadius && nextWaypoint != this)
                {
                    neighbours.Add(nextWaypoint);
                }
            }
        }
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, proximityRadius);
    }

    public SkeletonWaypoint NextWaypoint(SkeletonWaypoint previousWaypoint)
    {
        if(neighbours.Count == 0)
        {
            Debug.LogError("Need more waypoints");
            return null;
        }

        else if(neighbours.Count == 1 && neighbours.Contains(previousWaypoint))
        {
            return previousWaypoint;
        }

        else
        {
            SkeletonWaypoint nextWaypoint;

            int nextIndex = 0;

            do
            {
                nextIndex = UnityEngine.Random.Range(0, neighbours.Count);
                nextWaypoint = neighbours[nextIndex];

            } while (nextWaypoint == previousWaypoint);

            return nextWaypoint;
        }
    }
}
