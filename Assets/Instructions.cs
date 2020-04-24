using Rewired.Integration.UnityUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject rightArrowButton, leftArrowButton;

    public GameObject[] slides;
    private int currentSlideIndex;
    // delete after
    public GameObject controllerUI;
    public GameObject objective;

    [SerializeField] Image check1, check2;
    Rewired.Player gamePadController1, gamePadController2;
    [Range(0, 1)] [SerializeField] float checkTransparency = 0.3f;
    
    bool isReady = false;

    void Start()
    {
        gamePadController1 = Rewired.ReInput.players.GetPlayer(0);
        gamePadController2 = Rewired.ReInput.players.GetPlayer(1);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(rightArrowButton);
        
        setupSlides();

        // set value of game duration
        //setTimeDurationText();
    }

    private void setupSlides()
    {
        for (int i = 0; i < slides.Length; i++)
        {
            if (i == 0)
            {
                currentSlideIndex = 0;
                slides[i].SetActive(true);
            }
            else
            {
                slides[i].SetActive(false);
            }
        }

        leftArrowButton.GetComponent<Button>().interactable = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReady)
        {
            toggleConfirmation(check1, gamePadController1); // visual check for player 1
            toggleConfirmation(check2, gamePadController2); // visual check for player 2
        }

        // Go To Game Scene
        if (gamePadController1.GetButton("Menu Ready") && gamePadController2.GetButton("Menu Ready")) //|| Input.GetKeyDown(KeyCode.Return))
        {
            isReady = true;
            toggleConfirmation(check1, null, true); // visual check for player 1
            toggleConfirmation(check2, null, true); // visual check for player 2

            //IEnumerator fadeSound = AudioFadeOut.FadeOut(sound, 2.0f);
            //StartCoroutine(fadeSound);
            //SceneChanger.setSceneIndexSelected(SceneChanger.GAME);
            //FindObjectOfType<SceneChanger>().GetComponent<Animator>().SetTrigger("FadeOut");
            //gameObject.SetActive(false);
            FindObjectOfType<CanvasAnimationEvents>().GetComponent<Animator>().SetTrigger("Start Game");
        }

        // Go Back To Main Menu Scene
        /*
        if (gamePadController1.GetButtonDown("Camera Flip") || gamePadController2.GetButtonDown("Camera Flip"))
        {
            SceneChanger.setSceneIndexSelected(SceneChanger.MENU);
            FindObjectOfType<SceneChanger>().GetComponent<Animator>().SetTrigger("FadeOut");
        }
        */

        instructionsScroll();
    }

    private void instructionsScroll()
    {
        if (gamePadController1.GetButtonDown("Menu Left") || gamePadController2.GetButtonDown("Menu Left"))
        {
            goLeft();
 
        }

        if (gamePadController1.GetButtonDown("Menu Right") || gamePadController2.GetButtonDown("Menu Right"))
        {
            goRight();
        }
    }

    public void goLeft()
    {
        Debug.Log("Press Left arrow");
        if (currentSlideIndex > 0)
        {
            slides[currentSlideIndex].SetActive(false);
            currentSlideIndex--;
            slides[currentSlideIndex].SetActive(true);
        }

        if (currentSlideIndex == 0)
        {
            leftArrowButton.GetComponent<Button>().interactable = false;
            // select right arrow
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(rightArrowButton);
        } 
        else if (currentSlideIndex < slides.Length - 1)
        {
            rightArrowButton.GetComponent<Button>().interactable = true;
        }

        // activate controller map, deactivate objective
        //switchInstructionContent(true, false);
    }

    public void goRight()
    {
        // deactivate controller map, activate objective
        Debug.Log("Press right arrow");
        if (currentSlideIndex < slides.Length - 1)
        {
            slides[currentSlideIndex].SetActive(false);
            currentSlideIndex++;
            slides[currentSlideIndex].SetActive(true);
        }

        if (currentSlideIndex == slides.Length - 1)
        {
            // select left arrow
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(leftArrowButton);
            rightArrowButton.GetComponent<Button>().interactable = false;
        }
        else if (currentSlideIndex > 0 )
        {
            leftArrowButton.GetComponent<Button>().interactable = true;
        }

        //switchInstructionContent(false, true);
    }

    private void switchInstructionContent(bool controllerOn, bool objectiveOn)
    {
        GameObject arrowSelected = null;

        controllerUI.SetActive(controllerOn);
        objective.SetActive(objectiveOn);
        //rightArrowButton.GetComponent<Button>().interactable = controllerOn;
        //leftArrowButton.GetComponent<Button>().interactable = objectiveOn;

        if (controllerOn)
        {
            arrowSelected = rightArrowButton;
        } else
        {
            arrowSelected = leftArrowButton;
        }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(arrowSelected);
    }


    private void toggleConfirmation(Image check, Rewired.Player gamePadController, bool ready = false)
    {
        if (ready)
        {
            Color tempColor = check.color;
            tempColor.a = 1f;
            check.color = tempColor;
        }
        else
        {
            if (gamePadController.GetButton("Menu Ready"))
            {
                Color tempColor = check.color;
                tempColor.a = 1f;
                check.color = tempColor;
            }
            else
            {
                Color tempColor = check.color;
                tempColor.a = checkTransparency;
                check.color = tempColor;
            }
        }
    }

}
