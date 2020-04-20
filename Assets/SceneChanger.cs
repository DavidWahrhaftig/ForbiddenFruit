using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    // Scene Indexes 
    static public int MENU = 0;
    static public int GAME = 1;
    static public int QUIT = 2;
    static public int INSTRUCTIONS = 999; // not used anymore

    static public int sceneIndexSelected;


    //private bool changeScene = false;
    //private int sceneIndex = 0;
    //private Animator animator;

    void Start()
    {
        //animator = GetComponent<Animator>();
    }


    public void loadNextScene()
    {
        if (sceneIndexSelected == QUIT)
        {
            QuitGame();
        }
        else
        {
            SceneManager.LoadScene(sceneIndexSelected);
        }
    }   

    /* ------------------------------  */
    static public void setSceneIndexSelected(int flag)
    {
        
        sceneIndexSelected = flag;
        Debug.Log(sceneIndexSelected);
    }

    private void QuitGame()
    {
        Application.Quit();
    }


}
