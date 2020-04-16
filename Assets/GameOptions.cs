using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOptions : MonoBehaviour
{
    // Start is called before the first frame update

    public Toggle musicToggle, soundFXToggle;

    static bool musicOn = true;
    static bool soundFxOn = true;
    static int gameDuration = 210; // in seconds (multiple of 30 only)

    private int lowerLimit = 120;
    private int upperLimit = 300;

    private void Start()
    {
        if(Debug.isDebugBuild)
        {
            lowerLimit = 0;
        }

        if (musicToggle && soundFXToggle) // for main Menu Only
        {

            musicToggle.isOn = musicOn;
            soundFXToggle.isOn = soundFxOn;
            if (! musicOn) // for main menu only
            {
                muteBackgroundMusic();
            }
            else
            {
                unmuteBackgroundMusic();
            }

            if (! soundFxOn) // for main menu only
            {
                muteSoundFX();
            }
            else
            {
                unmuteSoundFX();
            }
        }
    }


    public void toggleMusic()
    {
        //musicOn = !musicOn;
        musicOn = musicToggle.isOn;
        Debug.Log(musicOn);

        if (! musicOn) // for main menu only
        {
            muteBackgroundMusic();
        }
        else
        {
            unmuteBackgroundMusic();
        }

        if (!soundFxOn) // for main menu only
        {
            muteSoundFX();
        }
        else
        {
            unmuteSoundFX();
        }


    }

    public void toggleSoundFx()
    {
        //soundFxOn = !soundFxOn;
        soundFxOn = soundFXToggle.isOn;
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

    static public bool isMusicOn()
    {
        return musicOn;
    }

    static public bool isSoundFxOn()
    {
        return soundFxOn;
    }

    static public void muteBackgroundMusic()
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

    static public void unmuteBackgroundMusic()
    {
        // for menu only 

        GameObject[] backgroundMusic = GameObject.FindGameObjectsWithTag("BG Sound");

        for (int i = 0; i < backgroundMusic.Length; i++)
        {
            AudioSource sound = backgroundMusic[i].GetComponent<AudioSource>();
            sound.volume = 0.2f;
        }
    }

    static public void muteSoundFX()
    {
        GameObject[] soundFX = GameObject.FindGameObjectsWithTag("Sound FX");

        for (int i = 0; i < soundFX.Length; i++)
        {
            AudioSource sound = soundFX[i].GetComponent<AudioSource>();
            sound.volume = 0f;
        }
    }

    static public void unmuteSoundFX()
    {
        // for main menu only
        GameObject[] soundFX = GameObject.FindGameObjectsWithTag("Sound FX");

        for (int i = 0; i < soundFX.Length; i++)
        {
            AudioSource sound = soundFX[i].GetComponent<AudioSource>();
            sound.volume = 0.3f;
        }
    }

    
}
