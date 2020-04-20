using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAnimationEvents : MonoBehaviour
{
    // Start is called before the first frame update

    public Instructions instructions;
    public Transform mainCamera;
    public Animator gateAnimator;

    public AudioSource instructionsAudio;

    private bool moveCamera = false;

    void Start()
    {
        if (instructions)
        {
            instructions.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (moveCamera)
        {
            mainCamera.transform.Translate(transform.forward * 0.08f, Space.World);
        }
    }

    public void buttonsInteractable()
    {
        //MainMenu.setAllButtonsInteractable(true);
        FindObjectOfType<MainMenu>().setMenuButtonsInteractable(true);
    }

    public void buttonsNotInteractable()
    {
        //MainMenu.setAllButtonsInteractable(true);
        FindObjectOfType<MainMenu>().setMenuButtonsInteractable(false);
    }


    public void activateInstructions()
    {
        //FindObjectOfType<Instructions>().GetComponent<GameObject>().SetActive(true);
        instructions.gameObject.SetActive(true);
        //instructions.playInstructionsAudio();
    }
    public void playInstructionsAudio()
    {
        //instructions.playInstructionsAudio();
        //FindObjectOfType<CameraOscillator>().setOscillating(false);
        instructionsAudio.Play();
    }

    public void stopCameraOscillation()
    {
        mainCamera.GetComponent<CameraOscillator>().setOscillating(false);
    }



    public void startGame()
    {
        enableMoveCamera();
        openGate();
    }

    public void enableMoveCamera()
    {
        mainCamera.GetComponent<CameraOscillator>().setOscillating(false);
        moveCamera = true;
    }


    public void openGate()
    {
        // open gate, move camera forwrad and fade out scene
        gateAnimator.SetTrigger("open"); 
    }

    public void sceneTransition()
    {
        SceneChanger.setSceneIndexSelected(SceneChanger.GAME);
        FindObjectOfType<SceneChanger>().GetComponent<Animator>().SetTrigger("FadeOut");
    }

    

}
