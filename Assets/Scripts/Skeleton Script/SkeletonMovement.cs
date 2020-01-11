using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonMovement : MonoBehaviour
{
    // Sets whether Skeleton goes idle at each waypoint
    [SerializeField]
    bool goIdleAtWaypoint;

    // total time to be idle at each waypoint
    [SerializeField]
    float totalIdleTime = 3f;

    // Likelihood of changing direction
    [SerializeField]
    float turnAroundProbability = 0.2f;


    NavMeshAgent navMeshAgent;
    SkeletonWaypoint currentTarget;
    SkeletonWaypoint prevTarget;


    private Animator animator;


    bool inMotion;
    bool idle;
    float idleTimer;
    int waypointsVisited;

    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();

        if (navMeshAgent == null)
        {
            Debug.LogError(gameObject.name + " is missing NavMesh");
        }
        else
        {
            if(currentTarget == null)
            {
                // Find all skeleton waypoints in the scene
                GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("SkeletonWaypoint");

                if(allWaypoints.Length > 0)
                {
                    while(currentTarget == null)
                    {
                        int firstIndex = UnityEngine.Random.Range(0, allWaypoints.Length);
                        SkeletonWaypoint startingWaypoint = allWaypoints[firstIndex].GetComponent<SkeletonWaypoint>();


                        if(startingWaypoint != null)
                        {
                            currentTarget = startingWaypoint;
                        }
                    }
                }
                else
                {
                    Debug.LogError("No waypoints in the scene");
                }

            }
            SetTarget();
             
        }
        
    }

    void Update()
    {
        // Check if close to destination
        if(inMotion && navMeshAgent.remainingDistance <= 1.0f)
        {
            inMotion = false;
            waypointsVisited++;

            // If Skeleton is set to go idle, they go idle
            if (goIdleAtWaypoint)
            {
                idle = true;
                idleTimer = 0f;
                if(animator != null)
                {
                    animator.SetBool("isIdle", true);
                    animator.SetBool("isWalking", false);
                }
                

            }

            else
            {
                SetTarget();
            }
        }

        // Count how long Skeleton is idle, then exit that state when timer is up
        if (idle && !navMeshAgent.isStopped)
        {
            idleTimer += Time.deltaTime;
            if(idleTimer >= totalIdleTime)
            {
                idle = false;

                SetTarget();
            }
        }
    }

    private void SetTarget()
    {
        if (waypointsVisited > 0)
        {
            SkeletonWaypoint nextTarget = currentTarget.NextWaypoint(prevTarget);
            prevTarget = currentTarget;
            currentTarget = nextTarget;
        }

        Vector3 targetVector = currentTarget.transform.position;
        navMeshAgent.SetDestination(targetVector);
        inMotion = true;
        if(animator != null)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isIdle", false);
        }
        
    }

    //// Set new waypoint from list with probability of backtracking
    //private void ChangeWaypoint()
    //{
    //    if(UnityEngine.Random.Range(0f, 1f) <= turnAroundProbability)
    //    {
    //        Debug.Log("TURN AROUND");
    //        goForward = !goForward;
    //    }

    //    if (goForward)
    //    {
    //        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
    //        Debug.Log("FORWARD TO " + currentWaypointIndex);
    //    }

    //    else if (--currentWaypointIndex < 0)
    //    {
    //        currentWaypointIndex = waypoints.Count - 1;
    //        Debug.Log("BACK TO " + currentWaypointIndex);
    //    }
    //}

}
