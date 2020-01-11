using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchesTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    public TorchPair[] torches = new TorchPair[3];
    public bool areTorchesRed = true;
    public float timeDelay = 1f;
    int currentIndex = 0;
    private bool firstTime = true;

    private void OnTriggerStay(Collider other)
    {
        if (FindObjectOfType<GameManager>().isGameStart())
        {
            if (other.tag == "Player1" && areTorchesRed && firstTime)
            {
                firstTime = false;
                Invoke("torchesChange", 0f);
            }
            else if (other.tag == "Player2" && !areTorchesRed && firstTime)
            {
                firstTime = false;
                Invoke("torchesChange", 0f);
            }
        }

    }

    private void torchesChange()
    {
        if (areTorchesRed)
        {
            torches[currentIndex].turnTorchesRed();
        }
        else
        {
            torches[currentIndex].turnTorchesBlue();
        }

        if (currentIndex < torches.Length - 1)
        {
            // there is at least another torch to turn ahead
            currentIndex++;
            Invoke("torchesChange", timeDelay);
        }


    }
}
