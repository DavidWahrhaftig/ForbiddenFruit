using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Animator gateAnimator;
    public Animator fadeAnimator;
    public Transform camera;


    bool moveCamera = false;
    private void Update()
    {
     
        if (moveCamera)
        {
            camera.transform.Translate(transform.forward * 0.08f, Space.World);
            fadeAnimator.SetTrigger("FadeOut");
            Invoke("OpenInstructions", 2.3f);
        }

    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenInstructions()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGameWithEntrance()
    {
        // open gate
        gateAnimator.SetTrigger("open");

        //
        camera.GetComponent<CameraOscillator>().isOscillating = false;
        moveCamera = true;

        
        // open gate, move camera forwrad and fade out scene
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
