using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Animator canvasAnimator;
    //public Animator gateAnimator;
    //public Transform camera;
    public GameObject optionsMenu;
    public GameObject creditsDisplay;

    public TextMeshProUGUI timeDurationText;
    public GameObject mainFirstButton, optionsFirstButton, optionsClosedButton, creditsCloseButton;

    public AudioSource titleVoice, instructionsAudio;

    private SceneChanger sceneChanger;
    private int nextSceneIndex = 0;
    //private bool moveCamera = false;

    private GameOptions gameOptions;

    private void Start()
    {
        gameOptions = FindObjectOfType<GameOptions>();
        sceneChanger = FindObjectOfType<SceneChanger>();
        optionsMenu.SetActive(false);
        creditsDisplay.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainFirstButton);
    }


    static public void setAllButtonsInteractable(bool b)
    {
        Button[] allButtons = FindObjectsOfType<Button>();

        for (int i = 0; i < allButtons.Length; i++)
        {
            Button button = allButtons[i];
            button.interactable = b;
        }
    }

    private void Update()
    {
     
        /*
        if (moveCamera)
        {
            camera.transform.Translate(transform.forward * 0.08f, Space.World);
        }
        */

        // set value of game duration
        setTimeDurationText();
    }

    /*
    private void transitionAnimation()
    {
        // open gate, move camera forwrad and fade out scene
        gateAnimator.SetTrigger("open");
        camera.GetComponent<CameraOscillator>().isOscillating = false;
        moveCamera = true;
    }
    */

    public void playTitleVoice()
    {
        titleVoice.Play();
    }

    public void PlayGame()
    {

        canvasAnimator.SetTrigger("To Instructions");

        //SceneChanger.setSceneIndexSelected(SceneChanger.INSTRUCTIONS);
        //FindObjectOfType<SceneChanger>().GetComponent<Animator>().SetTrigger("FadeOut");
        //transitionAnimation();
    }

    public void QuitGame()
    {
        SceneChanger.setSceneIndexSelected(SceneChanger.QUIT);
        FindObjectOfType<SceneChanger>().GetComponent<Animator>().SetTrigger("FadeOut");
    }

    public void openOptionsMenu()
    {
        optionsMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        optionsFirstButton.GetComponent<Button>().interactable = true;
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
        
    }

    public void closeOptionsMenu()
    {
        optionsMenu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainFirstButton);
        
    }

    public void openCredits()
    {
        creditsDisplay.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        creditsCloseButton.GetComponent<Button>().interactable = true;
        EventSystem.current.SetSelectedGameObject(creditsCloseButton);
    }

    public void closeCredits() {
        FindObjectOfType<GameOptions>().unmuteBackgroundMusic();
        FindObjectOfType<GameOptions>().unmuteSoundFX();

        creditsDisplay.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainFirstButton);
    }

    private void setTimeDurationText()
    {

        string minutes = ((int) gameOptions.getGameDuration() / 60).ToString();
        string seconds = (gameOptions.getGameDuration() % 60).ToString("f0");
        if (seconds == "0") { seconds = "00"; }
        timeDurationText.text = minutes + ":" + seconds;
    }

    public void playInstructionsAudio()
    {
        instructionsAudio.Play();
    }

    

}
