using Rewired;
using Rewired.Integration.UnityUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EndGameMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject endGameMenuUI;
    public GameObject firstButtonSelected;


    Rewired.Player player1, player2;

    void Start()
    {
        player1 = Rewired.ReInput.players.GetPlayer(0);
        player2 = Rewired.ReInput.players.GetPlayer(1);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButtonSelected);

        switchToMenuControlMaps();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void switchToMenuControlMaps()
    {
        // change controller maps
        RewiredStandaloneInputModule rewiredEventSystem = FindObjectOfType<RewiredStandaloneInputModule>();
        rewiredEventSystem.horizontalAxis = "Menu Horizontal";
        rewiredEventSystem.verticalAxis = "Menu Vertical";
        rewiredEventSystem.submitButton = "Submit";

        // player1 switch maps
        player1.controllers.maps.SetMapsEnabled(true, "Default", "EndMenu Joystick");
        player1.controllers.maps.SetMapsEnabled(false, "Default", "Default");
        player1.controllers.maps.SetMapsEnabled(true, ControllerType.Keyboard, "Keyboard1", "Menu Keyboard1");
        player1.controllers.maps.SetMapsEnabled(false, ControllerType.Keyboard, "Keyboard1", "Default");

        // player2 switch maps
        player2.controllers.maps.SetMapsEnabled(true, "Default", "EndMenu Joystick");
        player2.controllers.maps.SetMapsEnabled(false, "Default", "Default");
        player2.controllers.maps.SetMapsEnabled(true, "Keyboard2", "Menu Keyboard2");
        player2.controllers.maps.SetMapsEnabled(false, "Keyboard2", "Default");
    }


    public void playAgain()
    {
        SceneChanger.setSceneIndexSelected(SceneChanger.GAME);
        FindObjectOfType<SceneChanger>().GetComponent<Animator>().SetTrigger("FadeOut");
    }

    public void goToMenu()
    {
        SceneChanger.setSceneIndexSelected(SceneChanger.MENU);
        FindObjectOfType<SceneChanger>().GetComponent<Animator>().SetTrigger("FadeOut");
    }

    public void quitGame()
    {
        SceneChanger.setSceneIndexSelected(SceneChanger.QUIT);
        FindObjectOfType<SceneChanger>().GetComponent<Animator>().SetTrigger("FadeOut");
    }
}
