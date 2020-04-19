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
    static public int gameDuration = 210; // in seconds (multiple of 30 only)

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
            
            mainMenuCheckSound();
        }
    }

    public void toggleMusic()
    {
        //musicOn = !musicOn;
        musicOn = musicToggle.isOn;
        Debug.Log("BG Music: " + musicOn);

        mainMenuCheckSound();
    }

    public void toggleSoundFx()
    {
        //soundFxOn = !soundFxOn;
        soundFxOn = soundFXToggle.isOn;
        
        Debug.Log("SoundFx: " + soundFxOn);

        mainMenuCheckSound();
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

    public void muteBackgroundMusic()
    {
        // mute bg sounds

        GameObject[] backgroundMusic = GameObject.FindGameObjectsWithTag("BG Sound");

        for (int i = 0; i < backgroundMusic.Length; i++)
        {
            AudioSource sound = backgroundMusic[i].GetComponent<AudioSource>();
            //sound.volume = 0f;
            sound.mute = true;
        }
    }

    public void unmuteBackgroundMusic()
    {
        // for menu only 
        if (isMusicOn())
        {
            GameObject[] backgroundMusic = GameObject.FindGameObjectsWithTag("BG Sound");

            for (int i = 0; i < backgroundMusic.Length; i++)
            {
                AudioSource sound = backgroundMusic[i].GetComponent<AudioSource>();
                //sound.volume = 0.2f;
                sound.mute = false;
            }
        }
    }

    public void muteSoundFX()
    {
        GameObject[] soundFX = GameObject.FindGameObjectsWithTag("Sound FX");

        for (int i = 0; i < soundFX.Length; i++)
        {
            AudioSource sound = soundFX[i].GetComponent<AudioSource>();
            //sound.volume = 0f;
            sound.mute = true;
        }
    }

    public void unmuteSoundFX()
    {
        // for menu only
        if(isMusicOn())
        {
            GameObject[] soundFX = GameObject.FindGameObjectsWithTag("Sound FX");

            for (int i = 0; i < soundFX.Length; i++)
            {
                AudioSource sound = soundFX[i].GetComponent<AudioSource>();
                //sound.volume = 0.3f;
                sound.mute = false;
            }
        }
    }

    /* Only for main menu */

    private void mainMenuCheckSound()
    {
        if (!musicOn) // for main menu only
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
}
