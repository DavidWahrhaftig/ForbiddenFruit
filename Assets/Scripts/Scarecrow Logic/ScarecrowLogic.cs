using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScarecrowLogic : MonoBehaviour
{
    [SerializeField] bool isActive = true;
    [SerializeField] int snatchQuantity = 1;
    [SerializeField] float fruitSnatchTimeThreshold = 3f;


    private float fruitSnatchTimer = 0f;


    private void Start()
    {
        gameObject.SetActive(isActive);
    }



    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision Tag: " + other.transform.tag);
        if (other.transform.tag == "Player1" || other.transform.tag == "Player2")
        {
            if (canHarmPlayer(other.gameObject))
            {
                other.GetComponent<PlayerLogic>().loseFruits(snatchQuantity, true);
            }
        }

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player1" || other.transform.tag == "Player2")
        {
            if (canHarmPlayer(other.gameObject))
            {
                fruitSnatchTimer += Time.deltaTime;
                if (fruitSnatchTimer > fruitSnatchTimeThreshold)
                {
                    other.GetComponent<PlayerLogic>().loseFruits(snatchQuantity, true);
                    //Debug.Log("Is Stopped :: " + navMeshAgent.isStopped);
                    fruitSnatchTimer = 0f;
                }
            }
        }
    }

    private bool canHarmPlayer(GameObject player)
    {
        return !FindObjectOfType<GameManager>().isGameOver() && !player.GetComponent<PlayerLogic>().isCaught() && !player.GetComponent<PlayerLogic>().isDisabled();
    }

}
