using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int popUpIndex;


    public Transform player1;
    public Transform player2;
    public Transform witch;

    private Rewired.Player playerController1, playerController2;

    private float originalJumpForce;

    /** confirmation flags **/
    bool vertical1, vertical2 = false;
    bool horizontal1, horizontal2 = false;
    bool rotate1, rotate2 = false;
    bool jump1, jump2 = false;

    private void Start()
    {

        playerController1 = player1.GetComponent<PlayerController>().getGamePadController();
        playerController2 = player2.GetComponent<PlayerController>().getGamePadController();

        // don't allow player to jump
        originalJumpForce = player1.GetComponent<PlayerController>().jumpForce;
        player1.GetComponent<PlayerController>().jumpForce = 0f;
        player2.GetComponent<PlayerController>().jumpForce = 0f;
    }
    void Update()
    {

        for (int i = 0; i < popUps.Length; i++)
        {
            if (i == popUpIndex)
            {
                popUps[popUpIndex].SetActive(true);
            }
            else
            {
                popUps[popUpIndex].SetActive(false);
            }


        }

        switch (popUpIndex)
        {
            case 0: // move vertically

                vertical1 = Mathf.Abs(playerController1.GetAxis("Move Vertical")) > 0f;
                //checkPlayerVerticalMove(playerController1.GetButtonDown("Move Vertical"), isForward1);

                break;
            case 1: // move horizonally

                break;
            case 2: // rotate left

                break;
            case 3: // rotate right

                break;
            case 4: // jump
                player1.GetComponent<PlayerController>().jumpForce = originalJumpForce;
                player2.GetComponent<PlayerController>().jumpForce = originalJumpForce;

                break;
            case 5: // collect fruit

                break;
            case 6: // shoot light ball

                break;
        }


        if (popUpIndex == 0) /** move forward / back **/
        {
            bool player1Moved = false;
            bool player2Moved = false;

            if (player1.GetComponent<PlayerController>().getGamePadController().GetButtonDown("Move Vertical"))
            {
                player1Moved = true;
            }

            if (player2.GetComponent<PlayerController>().getGamePadController().GetButtonDown("Move Vertical"))
            {
                player2Moved = true;
            }

            if (player1Moved && player2Moved)/**both players have moved**/
            {
                popUpIndex++;
            }
        }

        else if (popUpIndex == 1) /***jump***/
        {

            bool player1Jumped = false;
            bool player2Jumped = false;

            if (player1.GetComponent<PlayerController>().getGamePadController().GetButtonDown("Move Vertical"))
            {
                player1Jumped = true;
            }

            if (player2.GetComponent<PlayerController>().getGamePadController().GetButtonDown("Move Vertical"))
            {
                player2Jumped = true;
            }

            if (player1Jumped && player2Jumped)/**both players have jumped**/
            {
                popUpIndex++;
            }
        }

        else if (popUpIndex == 2) /**flip camera**/
        {
            bool player1CamFlip = false;
            bool player2CamFlip = false;

            if (player1.GetComponent<PlayerController>().getGamePadController().GetButtonDown("Camera Flip"))
            {
                player1CamFlip = true;
            }

            if (player2.GetComponent<PlayerController>().getGamePadController().GetButtonDown("Camera Flip"))
            {
                player2CamFlip = true;
            }

            if (player1CamFlip && player2CamFlip)/**both players have flipped camera**/
            {
                popUpIndex++;
            }
        }

        else if (popUpIndex == 3) /**get fruit**/
        {
            bool player1GetFruit = false;
            bool player2GetFruit = false;

            if (player1.GetComponent<PlayerLogic>().getFruitCounter() == 1)
            {
                player1GetFruit = true;
            }

            if (player2.GetComponent<PlayerLogic>().getFruitCounter() == 1)
            {
                player2GetFruit = true;
            }

            if (player1GetFruit && player2GetFruit)/**both players have gotten 1 fruit**/
            {
                popUpIndex++;
            }
        }

        else if (popUpIndex == 4) /***shoot light ball***/
        {
            bool player1ShotBall = false;
            bool player2ShotBall = false;

            if (player1.GetComponent<SpawnLightOrb>().isShooting())
            {
                player1ShotBall = true;
            }

            if (player2.GetComponent<SpawnLightOrb>().isShooting())
            {
                player2ShotBall = true;
            }

            if (player1ShotBall && player2ShotBall)/**both players have thrown light ball**/
            {
                popUpIndex++;
            }
        }

        else if (popUpIndex == 5)
        {
            if (false)/**shake off the witch**/
            {
                popUpIndex++;
            }
        }

    }

    private void checkPlayerVerticalMove(bool condition, bool confirm)
    {
        if (condition)
        {
            confirm = true;
        }
    }
}
