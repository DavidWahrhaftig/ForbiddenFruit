using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject rightArrowButton, leftArrowButton;
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

        // set value of game duration
        //setTimeDurationText();
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
        if (gamePadController1.GetButton("Ready") && gamePadController2.GetButton("Ready") || Input.GetKeyDown(KeyCode.Return))
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
    }

    public void next()
    {
        controllerUI.SetActive(false);
        objective.SetActive(true);
        rightArrowButton.GetComponent<Button>().interactable = false;
        leftArrowButton.GetComponent<Button>().interactable = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(leftArrowButton);

    }

    public void back()
    {
        controllerUI.SetActive(true);
        objective.SetActive(false);
        rightArrowButton.GetComponent<Button>().interactable = true;
        leftArrowButton.GetComponent<Button>().interactable = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(rightArrowButton);
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
            if (gamePadController.GetButton("Ready"))
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
