using Rewired;
using Rewired.Integration.UnityUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu")]
    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject firstButtonSelected, controlsButton;

    [Header("Controls Submenu")]
    public GameObject[] controlSlides;
    private int currentControlSlideIndex = 0;
    public Button leftArrowButton, rightArrowButton;
    public GameObject controlsUI, controlsCloseButton;
    
    Rewired.Player player1, player2;
    bool pauseButton1, pauseButton2;

    



    // Start is called before the first frame update

    //private bool releasePlayerControl = true;
    void Start()
    {
        player1 = Rewired.ReInput.players.GetPlayer(0);
        player2 = Rewired.ReInput.players.GetPlayer(1);
        pauseMenuUI.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        pauseButton1 = player1.GetButtonDown("Pause");
        pauseButton2 = player2.GetButtonDown("Pause");

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
        closeControls();
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
        switchToMenuControlMaps();


        // select the first button

        PlayerController.pauseCheck = true;
        AudioListener.pause = true;
        pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        isGamePaused = true;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButtonSelected);
    }

    private void switchToMenuControlMaps()
    {
        // change controller maps
        RewiredStandaloneInputModule rewiredEventSystem = FindObjectOfType<RewiredStandaloneInputModule>();
        rewiredEventSystem.horizontalAxis = "Menu Horizontal";
        rewiredEventSystem.verticalAxis = "Menu Vertical";
        rewiredEventSystem.submitButton = "Submit";

        // player1 switch maps
        player1.controllers.maps.SetMapsEnabled(true, "Default", "Menu Joystick");
        player1.controllers.maps.SetMapsEnabled(false, "Default", "Default");
        player1.controllers.maps.SetMapsEnabled(true, ControllerType.Keyboard, "Keyboard1", "Menu Keyboard1");
        player1.controllers.maps.SetMapsEnabled(false, ControllerType.Keyboard, "Keyboard1", "Default");

        // player2 switch maps
        player2.controllers.maps.SetMapsEnabled(true, "Default", "Menu Joystick");
        player2.controllers.maps.SetMapsEnabled(false, "Default", "Default");
        player2.controllers.maps.SetMapsEnabled(true, "Keyboard2", "Menu Keyboard2");
        player2.controllers.maps.SetMapsEnabled(false, "Keyboard2", "Default");
    }

    private void switchToGameControlMaps()
    {
        // change controller maps
        RewiredStandaloneInputModule rewiredEventSystem = FindObjectOfType<RewiredStandaloneInputModule>();
        rewiredEventSystem.horizontalAxis = "Move Horizontal";
        rewiredEventSystem.verticalAxis = "Move Vertical";
        rewiredEventSystem.submitButton = "Jump";

        // player1 switch maps
        player1.controllers.maps.SetMapsEnabled(false, "Default", "Menu Joystick");
        player1.controllers.maps.SetMapsEnabled(true, "Default", "Default");
        player1.controllers.maps.SetMapsEnabled(false, ControllerType.Keyboard, "Keyboard1", "Menu Keyboard1");
        player1.controllers.maps.SetMapsEnabled(true, ControllerType.Keyboard, "Keyboard1", "Default");

        // player2 switch maps
        player2.controllers.maps.SetMapsEnabled(false, "Default", "Menu Joystick");
        player2.controllers.maps.SetMapsEnabled(true, "Default", "Default");
        player2.controllers.maps.SetMapsEnabled(false, "Keyboard2", "Menu Keyboard2");
        player2.controllers.maps.SetMapsEnabled(true, "Keyboard2", "Default");
    }

    IEnumerator resumePlayerControls()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerController.pauseCheck = false;
        switchToGameControlMaps();
    }

    public void goToMenu()
    {
        SceneChanger.setSceneIndexSelected(SceneChanger.MENU);
        FindObjectOfType<SceneChanger>().GetComponent<Animator>().SetTrigger("FadeOut");
        //releasePlayerControl = false;
        Resume();
        
    }

    public void showControls()
    {
        controlsUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(controlsCloseButton);
        setupControlsSlides();

    }

    private void setupControlsSlides()
    {
        for (int i = 0; i < controlSlides.Length; i ++)
        {
            if (i == 0)
            {
                controlSlides[i].SetActive(true);
                
            }
            else
            {
                controlSlides[i].SetActive(false);
            }            
        }

        leftArrowButton.interactable = false;

        // select right arrow
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(rightArrowButton.gameObject);

        if (controlSlides.Length > 0)
        {
            rightArrowButton.interactable = true;
        }
    }

    public void controlsLeft()
    {
        // go left if possible
        if (currentControlSlideIndex > 0)
        {
            //leftArrowButton.interactable = true;
            controlSlides[currentControlSlideIndex].SetActive(false);
            currentControlSlideIndex--;
            controlSlides[currentControlSlideIndex].SetActive(true);

        }

        // right only choice 
        if (currentControlSlideIndex == 0 )
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(rightArrowButton.gameObject);

            if (currentControlSlideIndex != controlSlides.Length - 1)
            {
                rightArrowButton.interactable = true;
            }

            leftArrowButton.interactable = false;
        }
    }

    public void controlsRight()
    {
        // go right if possible
        if (currentControlSlideIndex < controlSlides.Length - 1)
        {
            controlSlides[currentControlSlideIndex].SetActive(false);
            currentControlSlideIndex++;
            controlSlides[currentControlSlideIndex].SetActive(true);

        }

        // left only choice 
        if (currentControlSlideIndex == controlSlides.Length - 1)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(leftArrowButton.gameObject);

            if (currentControlSlideIndex != 0 )
            {
                leftArrowButton.interactable = true;
            }

            rightArrowButton.interactable = false;
        }
    }




    public void closeControls()
    {
        controlsUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(controlsButton);
        
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
