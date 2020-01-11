using System.Collections;
using UnityEngine;

public class PatrolBehaviour : StateMachineBehaviour //the instance of the StateMachineBehaviour lives inside an asset, not inside a scene.
{
    public int numOfPositions = 3;
    public float patrolRange = 4;
    public float patrolSpeedMin = 2;
    public float patrolSpeedMax = 4;

    [Range(0, 1)] public float rotationSpeed = 0.1f;
    public int waitTime = 2;
    public bool isPausing;

    private Vector3[] patrolPositions;
    private int randomIndex;
    private int counter;
    private bool finishedRotating;
    private Vector3 direction;
    private PatrolWait waitScript;

    WitchLogic witchLogic;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        witchLogic = animator.GetComponent<WitchLogic>();

        //witchLogic.playSound(witchLogic.patrollingSound);

        patrolPositions = getRandomPositions(animator.transform);
        randomIndex = Random.Range(0, numOfPositions - 1);
        waitScript = animator.GetComponent<PatrolWait>();
        counter = 0;
        isPausing = true;
        finishedRotating = false;
        waitScript.DoCoroutine(this); //pause

        direction = patrolPositions[randomIndex] - animator.transform.position;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        direction = patrolPositions[randomIndex] - animator.transform.position;

        if (!isPausing)
        {
            // keep moving until both x an z positions are at the patrol position, then move to the next position
            //if ( (Mathf.Abs(animator.transform.position.x - patrolPositions[randomIndex].x) <= Mathf.Epsilon) && (Mathf.Abs(animator.transform.position.z - patrolPositions[randomIndex].z) <= Mathf.Epsilon))
            if ( animator.transform.position.x != patrolPositions[randomIndex].x && animator.transform.position.z != patrolPositions[randomIndex].z)
            {
                if (!finishedRotating)
                {
                    animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, Quaternion.LookRotation(direction), rotationSpeed);
                    
                    // check if rotation complete 
                    if (Vector3.Angle(direction, animator.transform.forward) < .1)
                    {
                        // we're now facing the right direction
                        finishedRotating = true;
                    }
                }
                else
                {
                    //animator.transform.LookAt(patrolPositions[randomIndex]); //Quaternion.Slerp(animator.transform.rotation, Quaternion.LookRotation(direction), 0.1f)
                    animator.transform.position = Vector3.MoveTowards(animator.transform.position, patrolPositions[randomIndex], Random.Range(patrolSpeedMin, patrolSpeedMax) * Time.deltaTime);
                }
            }
            else
            {
                counter++;
                randomIndex = Random.Range(0, numOfPositions - 1); // randomly choose next position
                waitScript.DoCoroutine(this);
                finishedRotating = false;
                //direction = patrolPositions[randomIndex] - animator.transform.position;
                if (counter == numOfPositions)
                {
                    animator.SetBool("isIdle", true);
                    animator.SetBool("isPatrolling", false);
                }
            }
        }
    }

    private Vector3[] getRandomPositions(Transform origin)
    {
        Vector3[] patrolPositions = new Vector3[numOfPositions];

        float minX = origin.position.x - patrolRange;
        float maxX = origin.position.x + patrolRange;
        float minZ = origin.position.z - patrolRange;
        float maxZ = origin.position.z + patrolRange;

        for (int i = 0; i < numOfPositions; i++)
        {
            patrolPositions[i] = new Vector3(Random.Range(minX, maxX), origin.position.y, Random.Range(minZ, maxZ));
        }

        return patrolPositions;
    }
}
