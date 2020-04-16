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


    public void toggletMusic()
    {
        musicOn = !musicOn;
        Debug.Log(musicOn);
    }

    public void toggleSoundFx()
    {
        soundFxOn = !soundFxOn;
        Debug.Log(soundFxOn);
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
}
