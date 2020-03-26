using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] float sceneSwitchTime = 2.3f;

    private bool changeScene = false;
    private int sceneIndex = 0;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void goToScene(int nextSceneIndex, bool withFadeOut)
    {
        //changeScene = true;
        sceneIndex = nextSceneIndex;
        if (withFadeOut)
        {
            animator.SetTrigger("FadeOut");
            Invoke("nextScene", sceneSwitchTime);
        }
        else
        {
            nextScene();
        }
    }

    private void nextScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }   
}
