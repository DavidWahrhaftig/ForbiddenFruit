﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject firstButtonSelected;
    Rewired.Player gamePadController1, gamePadController2;
    bool pauseButton1, pauseButton2;
    // Start is called before the first frame update

    //private bool releasePlayerControl = true;
    void Start()
    {
        gamePadController1 = Rewired.ReInput.players.GetPlayer(0);
        gamePadController2 = Rewired.ReInput.players.GetPlayer(1);
        pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        pauseButton1 = gamePadController1.GetButtonDown("Pause");
        pauseButton2 = gamePadController2.GetButtonDown("Pause");

        if (pauseButton1 || pauseButton2 || Input.GetKeyDown(KeyCode.Escape))
        {
            if (! isGamePaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        AudioListener.pause = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;

        /*
        if (releasePlayerControl)
        {
            StartCoroutine(resumePlayerControls());
        }
        */
        StartCoroutine(resumePlayerControls());

    }

    public void Pause()
    {
        // select the first button

        PlayerController.pauseCheck = true;
        AudioListener.pause = true;
        pauseMenuUI.SetActive(true);
       
        Time.timeScale = 0f;
        isGamePaused = true;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButtonSelected);
    }

    IEnumerator resumePlayerControls()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerController.pauseCheck = false;
    }

    public void goToMenu()
    {
        SceneChanger.setSceneIndexSelected(SceneChanger.MENU);
        FindObjectOfType<SceneChanger>().GetComponent<Animator>().SetTrigger("FadeOut");
        //releasePlayerControl = false;
        Resume();
        
    }

    public void quitGame()
    {
        SceneChanger.setSceneIndexSelected(SceneChanger.QUIT);
        FindObjectOfType<SceneChanger>().GetComponent<Animator>().SetTrigger("FadeOut");
        //releasePlayerControl = false;
        Resume();
    }

    public void goToInstructions()
    {
        SceneChanger.setSceneIndexSelected(SceneChanger.INSTRUCTIONS);
        FindObjectOfType<SceneChanger>().GetComponent<Animator>().SetTrigger("FadeOut");
        //releasePlayerControl = false;
        Resume();
    }
}