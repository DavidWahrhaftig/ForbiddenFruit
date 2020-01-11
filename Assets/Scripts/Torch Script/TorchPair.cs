using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchPair : MonoBehaviour
{
    // Start is called before the first frame update

    public Torch[] torches = new Torch[2];

    public void turnTorchesBlue()
    {
        torches[0].makeFlameBlue();
        torches[1].makeFlameBlue();
    }

    public void turnTorchesRed()
    {
        torches[0].makeFlameRed();
        torches[1].makeFlameRed();
    }
}
