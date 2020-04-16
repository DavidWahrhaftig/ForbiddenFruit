using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptions : MonoBehaviour
{
    // Start is called before the first frame update

    static bool musicOn = true;
    static bool soundFxOn = true;
    static int gameDuration = 210; // in seconds (multiple of 30 only)

    private int lowerLimit = 120;
    private int upperLimit = 300;

    private void Update()
    {
        //turnSelectedSoundsOn();
    }


    public void toggleMusic()
    {
        musicOn = !musicOn;
        Debug.Log(musicOn);

        if (! musicOn) // for main menu only
        {
            muteBackgroundMusic();
        }
        else
        {
            unmuteBackgroundMusic();
        }

        
    }

    public void toggleSoundFx()
    {
        soundFxOn = !soundFxOn;
        Debug.Log(soundFxOn);

        if (!soundFxOn) // for main menu only
        {
            muteSoundFX();
        } 
        else
        {
            unmuteSoundFX();
        }
        
    }

    public void decreaseGameDuration()
    {
        if (gameDuration != lowerLimit)
        {
            gameDuration -= 30;
            
        }

        Debug.Log(gameDuration);
    }

    public void increaseGameDuration()
    {
        if (gameDuration != upperLimit)
        {
            gameDuration += 30;
        }

        Debug.Log(gameDuration);
    }

    public int getGameDuration()
    {
        return gameDuration;
    }

    public bool isMusicOn()
    {
        return musicOn;
    }

    public bool isSoundFxOn()
    {
        return soundFxOn;
    }

    public void muteBackgroundMusic()
    {
        // mute all sounds
        // unmute sound fx

        GameObject[] backgroundMusic = GameObject.FindGameObjectsWithTag("BG Sound");

        for (int i = 0; i < backgroundMusic.Length; i++)
        {
            AudioSource sound = backgroundMusic[i].GetComponent<AudioSource>();
            sound.volume = 0f;
        }
    }

    public void unmuteBackgroundMusic()
    {
        GameObject[] backgroundMusic = GameObject.FindGameObjectsWithTag("BG Sound");

        for (int i = 0; i < backgroundMusic.Length; i++)
        {
            AudioSource sound = backgroundMusic[i].GetComponent<AudioSource>();
            sound.volume = 0.2f;
        }
    }

    public void muteSoundFX()
    {
        GameObject[] soundFX = GameObject.FindGameObjectsWithTag("Sound FX");

        for (int i = 0; i < soundFX.Length; i++)
        {
            AudioSource sound = soundFX[i].GetComponent<AudioSource>();
            sound.volume = 0f;
        }
    }

    public void unmuteSoundFX()
    {
        GameObject[] soundFX = GameObject.FindGameObjectsWithTag("Sound FX");

        for (int i = 0; i < soundFX.Length; i++)
        {
            AudioSource sound = soundFX[i].GetComponent<AudioSource>();
            sound.volume = 0.3f;
        }
    }

    
}
