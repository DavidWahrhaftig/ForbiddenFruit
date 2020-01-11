using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    //public float movingSpeed = 20f;
    public float timeForNewPath;
    bool inCoRoutine = false;

    // chasing levers
    public Transform player;
    public int detectionDistance = 10;
    [Range(0f, 180f)]
    public float fieldOfvision = 30;
    public AudioClip chasingSound;

    private bool isChasing = false;

    AudioSource audioSoruce;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSoruce = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = player.position - this.transform.position; // the direction of the player
        float angle = Vector3.Angle(direction, this.transform.forward); // player angle

        if (Vector3.Distance(player.position, this.transform.position) < detectionDistance && angle < fieldOfvision) //  player must be within the given distance and angle from the witch
        {

            direction.y = 0f; // so enemy does not tip over
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0f); // align Witch with the player's position

            if (direction.magnitude > 1) // start chasing player until the length of distance is no more than specified
            {
                isChasing = true;
                // may need to stop the translation that is happening on the witch before
                this.transform.Translate(0, 0, 0.07f); // the witch translation towards the player

                if (!audioSoruce.isPlaying) // To avoid sound from layering over itself
                {
                    audioSoruce.PlayOneShot(chasingSound);
                }
                //print("chasing");
            }

        }
        else
        {

            isChasing = false;
            moveRandomly();
            //print("moving randomly");
        }
    }

    private void moveRandomly()
    {
        if (!inCoRoutine)
        {
            StartCoroutine(CoRoutineMoveRandomly());
        }
    }

    IEnumerator CoRoutineMoveRandomly()
    {
        inCoRoutine = true;

        getNewPath();
        yield return new WaitForSeconds(timeForNewPath);
        inCoRoutine = false;
    }

    void getNewPath()
    {
        navMeshAgent.SetDestination(getNewRandomPosition());
    }

    Vector3 getNewRandomPosition()
    {
        float x = Random.Range(-20, 20);
        float z = Random.Range(-20, 20);

        Vector3 pos = new Vector3(x, transform.position.y, z);
        return pos;
    }

}