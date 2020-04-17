using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsDisplayScript : MonoBehaviour
{
    //public AudioSource bgMusic;

    public void closeCredits()
    {
        FindObjectOfType<MainMenu>().closeCredits();
    }

    public void muteMusic()
    {
        FindObjectOfType<GameOptions>().muteBackgroundMusic();
        FindObjectOfType<GameOptions>().muteSoundFX();
    }
}
