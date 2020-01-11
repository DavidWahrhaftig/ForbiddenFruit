using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimatedSkeletonLogic : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    private int numPlayersInProximity;
    public float fruitSnatchTimeThreshold;
    private float fruitSnatchTimer;
    public int snatchQuantity;
    private Animator animator;
    private bool isIdleState;
    private bool isWalkingState;
    [Range(0, 1)] public float rotationSpeed = 0.1f;
    private Vector3 direction;
    private string playerTargetTag;
    private bool isPlayerInProximity;
    private AudioSource audioSource;
    public AudioClip hitSound;

    public float maxAudioDistance = 5f;
    private float initialVolume;
    private void Start()
    {
        //Debug.Log("NavMesh Registered");
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
        initialVolume = audioSource.volume;
    }

    private void Update()
    {
        if (getMinimumDistanceOfPlayer() < maxAudioDistance && !FindObjectOfType<GameManager>().isGameOver())
        {
            audioSource.volume = initialVolume * (1f - getMinimumDistanceOfPlayer() / maxAudioDistance);
        }
        else
        {
            audioSource.volume = 0f;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision Tag: " + other.transform.tag);
        if (!FindObjectOfType<GameManager>().isGameOver())
        {

            if (other.transform.tag == "Player1" || other.transform.tag == "Player2")
        {

            if (!(other.GetComponent<PlayerLogic>().isCaught() || other.GetComponent<PlayerLogic>().isDisabled()))
            {
                //Debug.Log("Skeleton spotted player! Stop!!!");
                navMeshAgent.isStopped = true;
                //Debug.Log("Is Stopped :: " + navMeshAgent.isStopped);
                numPlayersInProximity += 1;

                // Now can rotate to face Player (Need to implement lock on player)
                if (!isPlayerInProximity)
                {
                    isPlayerInProximity = true;
                    playerTargetTag = other.transform.tag;
                    direction = GameObject.FindGameObjectWithTag(playerTargetTag).transform.position - animator.transform.position;
                    animator.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed);
                    other.GetComponent<PlayerLogic>().loseFruits(snatchQuantity, true);
                    audioSource.PlayOneShot(hitSound);

                    // Turn on Attack animation
                    animator.SetBool("isAttacking", true);
                    isIdleState = animator.GetBool("isIdle");
                    isWalkingState = animator.GetBool("isWalking");
                    animator.SetBool("isIdle", false);
                    animator.SetBool("isWalking", false);

                }

            }
        }
        }

    }


    private void OnTriggerStay(Collider other)
    {

        if (!FindObjectOfType<GameManager>().isGameOver())
        {
            if (other.transform.tag == playerTargetTag)
            {
                direction = GameObject.FindGameObjectWithTag(playerTargetTag).transform.position - animator.transform.position;
                animator.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed);

                if (!(other.GetComponent<PlayerLogic>().isCaught() || other.GetComponent<PlayerLogic>().isDisabled()))
                {
                    fruitSnatchTimer += Time.deltaTime;
                    if (fruitSnatchTimer > fruitSnatchTimeThreshold)
                    {
                        other.GetComponent<PlayerLogic>().loseFruits(snatchQuantity, true);
                        audioSource.PlayOneShot(hitSound);
                        fruitSnatchTimer = 0;
                    }
                }

            }

            else if ((other.transform.tag == "Player1" || other.transform.tag == "Player2") && playerTargetTag == null)
            {
                playerTargetTag = other.transform.tag;
            }


        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!FindObjectOfType<GameManager>().isGameOver())
        {
            if (other.transform.tag == "Player1" || other.transform.tag == "Player2")
            {
                //Debug.Log("Skeleton lost sight of a player...");
                numPlayersInProximity -= 1;

                if (other.transform.tag == playerTargetTag)
                {
                    // Have to Unlock from player
                    playerTargetTag = null;
                }

                if (numPlayersInProximity == 0)
                {
                    //Debug.Log("No more players to see. Resume!");
                    navMeshAgent.isStopped = false;

                    isPlayerInProximity = false;

                    //Turn off attacking animation
                    animator.SetBool("isAttacking", false);
                    animator.SetBool("isIdle", isIdleState);
                    animator.SetBool("isWalking", isWalkingState);
                }
                fruitSnatchTimer = 0;
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    //Debug.Log("Collision Tag: " + other.transform.tag);

    //    if (collision.transform.tag == "Player1" || collision.transform.tag == "Player2")
    //    {

    //        collision.gameObject.GetComponent<PlayerLogic>().loseFruits(snatchQuantity, true);


    //    }
    //}

    private float getMinimumDistanceOfPlayer()
    {
        Transform player1 = FindObjectOfType<GameManager>().getPlayer(1);
        Transform player2 = FindObjectOfType<GameManager>().getPlayer(2);

        return Mathf.Min(Vector3.Distance(player1.position, transform.position), Vector3.Distance(player2.position, transform.position));
    }

}
