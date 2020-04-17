using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionsManager : MonoBehaviour
{
    [SerializeField] Rewired.Player gamePadController1, gamePadController2;

    [SerializeField] Animator fadeOut;
    [SerializeField] AudioSource sound;
    [SerializeField] Image check1, check2;

    [Range(0, 1)] [SerializeField] float checkTransparency = 0.3f;

    bool isReady = false;
    
    // Start is called before the first frame update
    void Start()
    {
        gamePadController1 = Rewired.ReInput.players.GetPlayer(0);
        gamePadController2 = Rewired.ReInput.players.GetPlayer(1);
    }

    // Update is called once per frame
    void Update()
    {

        if (!isReady)
        {
            //check1.SetActive(gamePadController1.GetButton("Jump")); // visual check for player 1
            //check2.SetActive(gamePadController2.GetButton("Jump")); // visual check for player 2
            toggleConfirmation(check1, gamePadController1); // visual check for player 1
            toggleConfirmation(check2, gamePadController2); // visual check for player 2
        }

        // Go To Game Scene
        if (gamePadController1.GetButton("Jump") && gamePadController2.GetButton("Jump") || Input.GetKeyDown(KeyCode.Return))
        {
            isReady = true;
            toggleConfirmation(check1, null, true); // visual check for player 1
            toggleConfirmation(check2, null, true); // visual check for player 2
            IEnumerator fadeSound = AudioFadeOut.FadeOut(sound, 2.0f);
            StartCoroutine(fadeSound);
            SceneChanger.setSceneIndexSelected(SceneChanger.GAME);
            FindObjectOfType<SceneChanger>().GetComponent<Animator>().SetTrigger("FadeOut");

        }
        
        // Go Back To Main Menu Scene
        if (gamePadController1.GetButtonDown("Camera Flip") || gamePadController2.GetButtonDown("Camera Flip"))
        {
            SceneChanger.setSceneIndexSelected(SceneChanger.MENU);
            FindObjectOfType<SceneChanger>().GetComponent<Animator>().SetTrigger("FadeOut");
        }
            
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
            if (gamePadController.GetButton("Jump"))
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
