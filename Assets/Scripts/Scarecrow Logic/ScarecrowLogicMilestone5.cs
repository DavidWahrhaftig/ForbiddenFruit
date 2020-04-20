using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScarecrowLogicMilestone5 : MonoBehaviour
{
    [SerializeField] int snatchQuantity = 1;
    [SerializeField] float fruitSnatchTimeThreshold = 6f;

    [SerializeField] Transform scarecrowRotation;
    [SerializeField] Animator scarecrowAnimator;

    [Range(0, 1)] public float rotationSpeed = 0.1f;
    private Vector3 direction;

    public AudioSource audioSource;
    private float fruitSnatchTimer = 0f;
    public float maxAudioDistance = 7f;
    private float initialVolume;

    private Transform player1, player2, lastPlayerEntered;

    public bool allowVisibleCheck = false;


    private void Start()
    {
        //gameObject.SetActive(isActive);
        //audioSource = GetComponent<AudioSource>();
        initialVolume = audioSource.volume;

        player1 = FindObjectOfType<GameManager>().getPlayer(1);
        player2 = FindObjectOfType<GameManager>().getPlayer(2);
        //GetComponent<Renderer>().enabled = false;
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

        checkVisibilitytoPlayers();
    }

    private void checkVisibilitytoPlayers()
    {
        //if (player1.GetComponent<PlayerLogic>().iCanSee(gameObject) && allowVisibleCheck)
        if (player1.GetComponent<PlayerLogic>().isVisibleByCamera(gameObject) && allowVisibleCheck)
        {
            Debug.Log("Player 1 can see me!");
        }
        else if (allowVisibleCheck)
        {
            Debug.Log(" --- Player 1 CANNOT see me! ---");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision Tag: " + other.transform.tag);
        if (other.transform.tag == player1.tag || other.transform.tag == player2.tag)
        {
            lastPlayerEntered = other.transform; // incase both players enter the scarecrow radius

            scarecrowAnimator.SetTrigger("scare");
            if (canHarmPlayer(other.gameObject))
            {
                //other.GetComponent<PlayerLogic>().loseFruits(snatchQuantity, true);
                other.GetComponent<PlayerLogic>().activateScarecorwSpell();
                //other.GetComponent<Animator>().SetBool("scarecrowSpellOn", true);
            }
        }

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == player1.tag || other.transform.tag == player2.tag)
        {

            direction = lastPlayerEntered.position - scarecrowRotation.position;
            scarecrowRotation.rotation = Quaternion.Slerp(scarecrowRotation.rotation, Quaternion.LookRotation(direction), rotationSpeed);

            if (canHarmPlayer(other.gameObject))
            {
                fruitSnatchTimer += Time.deltaTime;
                if (fruitSnatchTimer > fruitSnatchTimeThreshold)
                {
                    //other.GetComponent<PlayerLogic>().loseFruits(snatchQuantity, true);
                    other.GetComponent<PlayerLogic>().activateScarecorwSpell();
                    //other.GetComponent<Animator>().SetBool("scarecrowSpellOn", true);
                    //Debug.Log("Is Stopped :: " + navMeshAgent.isStopped);
                    fruitSnatchTimer = 0f;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == player1.tag || other.transform.tag == player2.tag)
        {
            scarecrowAnimator.SetTrigger("idle");
        }
        fruitSnatchTimer = 0;

    }
    private bool canHarmPlayer(GameObject player)
    {
        GameManager gameManger = FindObjectOfType<GameManager>();
        PlayerLogic playerLogic = player.GetComponent<PlayerLogic>();
        return !gameManger.isGameOver() && playerLogic.canBeChased && !playerLogic.isCaught(); //!playerLogic.isCaught() && playerLogic.canBeChased;
    }

    
    private float getMinimumDistanceOfPlayer()
    {
        return Mathf.Min(Vector3.Distance(player1.position, transform.position), Vector3.Distance(player2.position, transform.position));
    }

}
