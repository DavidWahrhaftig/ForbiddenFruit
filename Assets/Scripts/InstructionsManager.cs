using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsManager : MonoBehaviour
{
    [SerializeField] Rewired.Player gamePadController1, gamePadController2;

    [SerializeField] Animator fadeOut;
    [SerializeField] GameObject check1, check2;

    bool playGame = false;
    
    // Start is called before the first frame update
    void Start()
    {
        gamePadController1 = Rewired.ReInput.players.GetPlayer(0);
        gamePadController2 = Rewired.ReInput.players.GetPlayer(1);
    }

    // Update is called once per frame
    void Update()
    {

        if (playGame)
        {
            check1.SetActive(true); // visual check for player 1
            check2.SetActive(true); // visual check for player 2
        } else
        {
            check1.SetActive(gamePadController1.GetButton("Jump")); // visual check for player 1
            check2.SetActive(gamePadController2.GetButton("Jump")); // visual check for player 2
        }


        if (gamePadController1.GetButton("Jump") && gamePadController2.GetButton("Jump") || Input.GetKeyDown(KeyCode.Return))
        {
            playGame = true;
            fadeOut.SetTrigger("FadeOut");
            Invoke("goToGameScene", 2.3f);
        }
            
        if (gamePadController1.GetButtonDown("Camera Flip") || gamePadController2.GetButtonDown("Camera Flip"))
        {
            fadeOut.SetTrigger("FadeOut");
            Invoke("goToMenuScene", 2.3f);
        }
            
    }

    void goToGameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void goToMenuScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
