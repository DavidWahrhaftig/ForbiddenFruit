using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Animator gateAnimator;
    public Transform camera;

    private SceneChanger sceneChanger;
    private int nextSceneIndex = 0;

    private bool moveCamera = false;

    private void Start()
    {
        sceneChanger = FindObjectOfType<SceneChanger>();
    }
    private void Update()
    {
     
        if (moveCamera)
        {
            camera.transform.Translate(transform.forward * 0.08f, Space.World);
            sceneChanger.goToScene(nextSceneIndex, true);            
        }
    }

    private void transitionAnimation()
    {
        // open gate, move camera forwrad and fade out scene
        gateAnimator.SetTrigger("open");
        camera.GetComponent<CameraOscillator>().isOscillating = false;
        moveCamera = true;
    }

    public void PlayGameWithEntrance()
    {
        nextSceneIndex = 1;
        transitionAnimation();
    }


    public void CreditsWithEntrance()
    {
        nextSceneIndex = 3;  
        transitionAnimation();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
