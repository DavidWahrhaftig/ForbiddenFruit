using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGateScript : MonoBehaviour
{
    // Start is called before the first frame update



    [SerializeField] GameObject gateBlock;

    [SerializeField] AudioClip openSound, closeSound;
    [SerializeField] float closingDistance = 5f;

    AudioSource audioSource;

    bool isGateOpen = false;
    bool isGateClosing = false;

    bool insidePlayer1 = false;
    bool insidePlayer2 = false;

    Transform player1, player2;

    void Start()
    {
        gateBlock.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        player1 = FindObjectOfType<GameManager>().getPlayer(1);
        player2 = FindObjectOfType<GameManager>().getPlayer(2);
    }

    // Update is called once per frame
    void Update()
    {
        if (insidePlayer1 && insidePlayer2 && isGateOpen && areBothPlayerFarEnough())
        {
            closeGate();
        }
    }

    private bool areBothPlayerFarEnough()
    {
        return Vector3.Distance(player1.position, transform.position) > closingDistance && Vector3.Distance(player2.position, transform.position) > closingDistance;
    }
    
    public void setInsidePlayer1(bool b)
    {
        insidePlayer1 = b;
    }

    public void setInsidePlayer2(bool b)
    {
        insidePlayer2 = b;
    }


    public void openGate()
    {
        
        if (!isGateOpen)
        {
            isGateOpen = true;
            
            gateBlock.SetActive(false);
            GetComponent<Animator>().SetTrigger("open");

            // play open gate sound
            audioSource.PlayOneShot(openSound);
            FindObjectOfType<WitchLogic>().playDelayedWitchWelcomeSound();
        }
        
    }

    public void closeGate()
    {
        if (!isGateClosing)
        {
            isGateClosing = true;
            isGateOpen = false;
            gateBlock.SetActive(true);
            GetComponent<Animator>().SetTrigger("close");

            //play close gate sound
            audioSource.PlayOneShot(closeSound);
            
        }
    }

 

}
