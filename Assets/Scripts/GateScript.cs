using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{

    Animator gateAnim;
    bool playedSound = false;

    // Start is called before the first frame update
    void Start()
    {
        gateAnim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player1" || other.transform.tag == "Player2")
        {
            gateAnim.SetTrigger("OpenGate");
            if (!playedSound)
            {
                //FindObjectOfType<GameManager>().playSound(FindObjectOfType<GameManager>().gateOpenSound);
                playedSound = true;
            }

        }
            
    }

    private void HoldDoorOpen()
    {
        gateAnim.enabled = false;
    }
}
