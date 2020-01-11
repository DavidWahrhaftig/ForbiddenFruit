using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideGate : MonoBehaviour
{
    // Start is called before the first frame update

    NewGateScript gateScript;

    void Start()
    {
        gateScript = GetComponentInParent<NewGateScript>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1")
        {
            gateScript.setInsidePlayer1(false);
            gateScript.openGate();


        } else if (other.gameObject.tag == "Player2")
        {
            gateScript.setInsidePlayer2(false);
            gateScript.openGate();
        }

        

    }
}
