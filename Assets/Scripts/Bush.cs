using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
        {
            PlayerLogic playerLogic = other.GetComponent<PlayerLogic>();
            playerLogic.setHidden(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
        {
            PlayerLogic playerLogic = other.GetComponent<PlayerLogic>();
            playerLogic.setHidden(false);
        }

    }
}
