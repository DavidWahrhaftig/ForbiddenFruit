using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsDisplayScript : MonoBehaviour
{
    //public AudioSource bgMusic;
    public AudioSource creditsMusic, bgMusic;
    public void closeCredits()
    {
        FindObjectOfType<MainMenu>().closeCredits();
        creditsMusic.Stop();
        if (GameOptions.isMusicOn())
        {
            bgMusic.mute = false;
        }
        
    }

    public void playCreditsMusic()
    {
        if (GameOptions.isMusicOn())
        {
            creditsMusic.Play();
            bgMusic.mute = true;
        }       
    }
}
