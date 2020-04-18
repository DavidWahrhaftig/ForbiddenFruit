using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAnimationEvents : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MainMenu.setAllButtonsInteractable(false);
    }

    public void buttonsInteractable()
    {
        MainMenu.setAllButtonsInteractable(true);
    }

    public void buttonsNotInteractable()
    {
        MainMenu.setAllButtonsInteractable(false);
    }

    public void playInstructionsAudio()
    {
        FindObjectOfType<MainMenu>().playInstructionsAudio();
        FindObjectOfType<CameraOscillator>().setOscillating(false);
    }
}
