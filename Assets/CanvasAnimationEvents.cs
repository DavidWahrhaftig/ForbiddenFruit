using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAnimationEvents : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject instructionsObject;
    public Transform mainCamera;
    public Animator gateAnimator;

    private bool moveCamera = false;

    void Start()
    {
        instructionsObject.SetActive(false);
        MainMenu.setAllButtonsInteractable(false);
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
        MainMenu.setAllButtonsInteractable(true);
    }

    public void buttonsNotInteractable()
    {
        MainMenu.setAllButtonsInteractable(false);
    }

    public void playInstructionsAudio()
    {
        FindObjectOfType<MainMenu>().playInstructionsAudio();
        //FindObjectOfType<CameraOscillator>().setOscillating(false);
    }

    public void stopCameraOscillation()
    {
        mainCamera.GetComponent<CameraOscillator>().setOscillating(false);
    }

    public void activateInstructions()
    {
        //FindObjectOfType<Instructions>().GetComponent<GameObject>().SetActive(true);
        instructionsObject.SetActive(true);
    }

    public void enableMoveCamera()
    {
        mainCamera.GetComponent<CameraOscillator>().setOscillating(false);
        moveCamera = true;
    }

    private void openGate()
    {
        // open gate, move camera forwrad and fade out scene
        gateAnimator.SetTrigger("open"); 
    }

}
