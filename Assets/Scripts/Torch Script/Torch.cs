using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] ParticleSystem greenFire, redFire, blueFire;

    private ParticleSystem currentFlame;
    void Start()
    {
        currentFlame = greenFire;
        currentFlame.gameObject.SetActive(false);
        redFire.gameObject.SetActive(false);
        blueFire.gameObject.SetActive(false);

    }

    
    public void makeFlameRed()
    {
        currentFlame.gameObject.SetActive(false);
        currentFlame = redFire;
        currentFlame.gameObject.SetActive(true);
        GetComponent<AudioSource>().Play();
    }

    public void makeFlameBlue()
    {
        currentFlame.gameObject.SetActive(false);
        currentFlame = blueFire;
        currentFlame.gameObject.SetActive(true);
        GetComponent<AudioSource>().Play();
    }

}
