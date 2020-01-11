using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] AudioClip last30SecondsTrack;
    [SerializeField] AudioClip gameOverTrack;
    [SerializeField] AudioSource merlinAudio;

    [SerializeField] AudioClip merlinLast30Second, merlinGameOver, merlinTieGame;

    GameManager gameManager;
    AudioSource audioSource;

    bool isPlayingMerlinSound = false;
    bool playedMerlinEndPhrase = false;
    

    bool isLast30SecondsTrackPlaying = false;
    bool isGamOverTrackPlaying = false;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (gameManager.getTimeRemaining() <= 30f)
        {
            // play the last 30 seconds track
            if (!isLast30SecondsTrackPlaying)
            {
                isLast30SecondsTrackPlaying = true;
                audioSource.Stop();
                audioSource.PlayOneShot(last30SecondsTrack);
            }
           
        }

        if (gameManager.getTimeRemaining() <= 27f && !isPlayingMerlinSound && !gameManager.isGameOver())
        {
            isPlayingMerlinSound = true;
            merlinAudio.PlayOneShot(merlinLast30Second);
   
        }

        if (gameManager.isGameOver())
        {
            if (gameManager.getPlayer(1).GetComponent<PlayerLogic>().isCaught() || gameManager.getPlayer(2).GetComponent<PlayerLogic>().isCaught())
            {
                // wait until overtime finishes
            }
            else
            {
                if (!playedMerlinEndPhrase)
                {
                    // say the phrase one time
                    playedMerlinEndPhrase = true;
                    

                    if (gameManager.getPlayer(1).GetComponent<PlayerLogic>().getFruitCounter() != gameManager.getPlayer(2).GetComponent<PlayerLogic>().getFruitCounter())
                    {
                        Invoke("delayMerlinGameOverPhrase", 2f); // a nice delay
                    }
                    else
                    {
                        Invoke("delayMerlinTiePhrase", 2f);
                    }
                }
            }
            
            if (!isGamOverTrackPlaying)
            {
                isGamOverTrackPlaying = true;
                audioSource.Stop();
                audioSource.clip = gameOverTrack;
                audioSource.Play();
            }
        }

    }

    void delayMerlinGameOverPhrase()
    {
        merlinAudio.PlayOneShot(merlinGameOver);
    }

    void delayMerlinTiePhrase()
    {
        merlinAudio.PlayOneShot(merlinTieGame);
    }
}
