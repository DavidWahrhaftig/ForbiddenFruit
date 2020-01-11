using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonLogic : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    private int playersInProximity;
    [SerializeField]
    private float fruitSnatchTimeThreshold = 1f;
    private float fruitSnatchTimer;
    public int snatchQuantity;

    private void Start()
    {
        //Debug.Log("NavMesh Registered");
        navMeshAgent = this.GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision Tag: " + other.transform.tag);
        
        if (other.transform.tag == "Player1" || other.transform.tag == "Player2")
        {

            if (!FindObjectOfType<GameManager>().isGameOver() && !other.GetComponent<PlayerLogic>().isCaught() || !other.GetComponent<PlayerLogic>().isDisabled())
            {
                other.GetComponent<PlayerLogic>().loseFruits(snatchQuantity, true);


                //Debug.Log("Skeleton spotted player! Stop!!!");
                navMeshAgent.isStopped = true;
                //Debug.Log("Is Stopped :: " + navMeshAgent.isStopped);
                playersInProximity += 1;
            }
        }

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player1" || other.transform.tag == "Player2")
        {
            if (!FindObjectOfType<GameManager>().isGameOver() && !other.GetComponent<PlayerLogic>().isCaught() || !other.GetComponent<PlayerLogic>().isDisabled())
            {
                fruitSnatchTimer += Time.deltaTime;
                if (fruitSnatchTimer > fruitSnatchTimeThreshold)
                {
                    other.GetComponent<PlayerLogic>().loseFruits(snatchQuantity, true);
                    //Debug.Log("Is Stopped :: " + navMeshAgent.isStopped);
                    fruitSnatchTimer = 0;
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player1" || other.transform.tag == "Player2")
        {
            if (!FindObjectOfType<GameManager>().isGameOver() && !other.GetComponent<PlayerLogic>().isCaught() || !other.GetComponent<PlayerLogic>().isDisabled())
            {
                //Debug.Log("Skeleton lost sight of a player...");
                playersInProximity -= 1;

                if (playersInProximity == 0)
                {
                    //Debug.Log("No more players to see. Resume!");
                    navMeshAgent.isStopped = false;
                    //Debug.Log("Is Stopped :: " + navMeshAgent.isStopped);
                }
            }
        }
    }

}
