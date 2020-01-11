using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class GameManager : MonoBehaviour
{
    /***UI Texts ***/
    [Header("UI Settings")]
    public TextMeshProUGUI fruitCounter1;
    public TextMeshProUGUI fruitCounter2;
    public TextMeshProUGUI shotTimer1;
    public TextMeshProUGUI shotTimer2;
    public TextMeshProUGUI GameTimer;
    public TextMeshProUGUI gameResult1;
    public TextMeshProUGUI gameResult2;
    public TextMeshProUGUI countDownUI;
    public Slider magicSlider1;
    public Slider magicSlider2;
    public Slider resistanceSlider1;
    public Slider resistanceSlider2;

    public GameObject resistancePrompt1;
    public GameObject resistancePrompt2;
    public GameObject restartPrompt;

    bool seenResistancePrompt1 = false;
    bool seenResistancePrompt2 = false;


    //public Image imageSlider1;
    //public Image imageSlider2;


    [Header("Time Settings")]
    public float gameDuration = 60;
    public float restartDelay = 1f;
    public float countDownDuration = 7.0f;

    [Header("Game Objects Settings")]
    public Transform player1;
    public Transform player2;
    public Transform witch;


    private float startTime;
    private AudioSource audioSource;

    private float remainingTime;

    private float startCounterTime;

    private float remainingCountDownTime;
    private bool beginGame = false;
    private bool gameOver = false;
    public bool overTime = false;

    private bool flippedCameraEnd = false;

    void Awake()
    {

        restartPrompt.SetActive(false);
        //restartPrompt.GetComponent<Image>().material.color.a = 0f;

        remainingTime = gameDuration;
        remainingCountDownTime = countDownDuration;

        // set initial game UI
        fruitCounter1.text = "0";
        fruitCounter2.text = "0";
        //shotTimer1.text = "Orb Ammo: " + player1.GetComponent<SpawnLightOrb>().getAmmo();
        //shotTimer2.text = "Orb Ammo: " + player2.GetComponent<SpawnLightOrb>().getAmmo();
        shotTimer1.text = "Magic Meter";
        shotTimer2.text = "Magic Meter";
        gameResult1.text = "";
        gameResult2.text = "";
        GameTimer.text = "";

        startCounterTime = Time.time;
               
        // disable both players
        //player1.GetComponent<PlayerLogic>().disableControls();
        //player2.GetComponent<PlayerLogic>().disableControls();

        resistanceSlider1.gameObject.SetActive(false);
        resistanceSlider2.gameObject.SetActive(false);

        resistancePrompt1.SetActive(false);
        resistancePrompt2.SetActive(false);

        Physics.IgnoreCollision(player1.GetComponent<Collider>(), player2.GetComponent<Collider>());
    }

    void Update()
    {


        // Force Restart from Keyboard
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        // Quit Game from Keyboard
        if (Input.GetKey(KeyCode.Q))
        {
            Application.Quit();
        }

        /*** UI Updates ***/

        startCountDown();

        // updating Ammo sliders
        updateSliders();

        if (beginGame)
        {
            fruitCounter1.text = player1.GetComponent<PlayerLogic>().getFruitCounter().ToString();
            fruitCounter2.text = player2.GetComponent<PlayerLogic>().getFruitCounter().ToString();

            updateTimer();
        }

        resistanceSlider1.value = player1.GetComponentInChildren<CameraShake>().getResistanceMeter();
        resistanceSlider2.value = player2.GetComponentInChildren<CameraShake>().getResistanceMeter();

        radarActivation();

    }

    private void radarActivation()
    {
        if (gameOver)
        {
            player1.GetComponent<PlayerLogic>().setRadarOn(false);
            player2.GetComponent<PlayerLogic>().setRadarOn(false);
            return;
        }

        if (player1.GetComponent<PlayerLogic>().fruitCounter > player2.GetComponent<PlayerLogic>().fruitCounter) // player 2 is behind
        {
            player1.GetComponent<PlayerLogic>().setRadarOn(false);
            player2.GetComponent<PlayerLogic>().setRadarOn(true);
        }
        else if (player1.GetComponent<PlayerLogic>().fruitCounter < player2.GetComponent<PlayerLogic>().fruitCounter) // player 1 is behind
        {
            player1.GetComponent<PlayerLogic>().setRadarOn(true);
            player2.GetComponent<PlayerLogic>().setRadarOn(false);
        }
        else // tied
        {
            player1.GetComponent<PlayerLogic>().setRadarOn(false);
            player2.GetComponent<PlayerLogic>().setRadarOn(false);
        }
    }

    private void updateSliders()
    {
        magicSlider1.value = player1.GetComponent<SpawnLightOrb>().magicCharge / 100f;
        magicSlider2.value = player2.GetComponent<SpawnLightOrb>().magicCharge / 100f;
    }

    private void updateTimer()
    {
        remainingTime = gameDuration - (Time.time - startTime);

        string minutes = ((int)remainingTime / 60).ToString();
        string seconds = (remainingTime % 60).ToString("f0");

        if (seconds == "60")
        {
            seconds = "0";
            minutes = ((int)Math.Ceiling(remainingTime) / 60).ToString();
        }



        // winner detection
        if (remainingTime <= Mathf.Epsilon)
        {
            // is overtime?

            gameOver = true;

            if (player1.GetComponent<PlayerLogic>().isCaught() || player2.GetComponent<PlayerLogic>().isCaught())
            {
                overTime = true;
                GameTimer.text = "Over Time";

            } else
            {
                overTime = false;
                restartPrompt.SetActive(true);
                witch.GetComponent<WitchLogic>().gameOver();
            }



            if (!overTime)
            {
                // flip cameras when game is over
                flipPlayerCameras();
                GameTimer.text = "Game Over";
                //magicSlider1.gameObject.SetActive(false);
                //magicSlider2.gameObject.SetActive(false);

                if (player1.GetComponent<PlayerLogic>().getFruitCounter() > player2.GetComponent<PlayerLogic>().getFruitCounter())
                {
                    gameResult1.text = "Merlin's Apprentice";
                    gameResult2.text = "Unemployed";
                    player1.GetComponent<PlayerController>().won();
                    player2.GetComponent<PlayerController>().lose();


                }
                else if (player1.GetComponent<PlayerLogic>().getFruitCounter() < player2.GetComponent<PlayerLogic>().getFruitCounter())
                {
                    gameResult2.text = "Merlin's Apprentice";
                    gameResult1.text = "Unemployed";
                    player1.GetComponent<PlayerController>().lose();
                    player2.GetComponent<PlayerController>().won();
                }
                else
                {
                    gameResult1.text = "No One Likes Ties";
                    gameResult2.text = "No One Likes Ties";
                    player1.GetComponent<PlayerController>().lose();
                    player2.GetComponent<PlayerController>().lose();
                }



                // restart game when pressing Y or triangle button
                if (player1.GetComponent<PlayerController>().getGamePadController().GetButtonDown("Restart") || player2.GetComponent<PlayerController>().getGamePadController().GetButtonDown("Restart"))
                {
                    Restart();
                }
            }
            else
            {
                player1.GetComponent<PlayerLogic>().disableControls();
                player2.GetComponent<PlayerLogic>().disableControls();
            }
        }
        else
        {
            if (Int32.Parse(seconds) < 10) { seconds = "0" + seconds; }

            GameTimer.text = minutes + ":" + seconds;
        }
    }

    private void flipPlayerCameras()
    {
        if (!flippedCameraEnd)
        {
            flippedCameraEnd = true;
            player1.GetComponentInChildren<CameraShake>().GetComponentInChildren<CameraController>().flipCamera();
            player2.GetComponentInChildren<CameraShake>().GetComponentInChildren<CameraController>().flipCamera();
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(0); //game scene at index 0 until we get the menu scene
    }

    public Transform getWitch()
    {
        return this.witch;
    }


    private void startCountDown()
    {

        remainingCountDownTime = countDownDuration - (Time.time - startCounterTime);

        string seconds = remainingCountDownTime.ToString("f0");


        if (remainingCountDownTime > Mathf.Epsilon)
        {

            countDownUI.text = "Game Begins in \n" + seconds;

        }
        else
        {
            countDownUI.text = "";
            if (!beginGame)
            {
                startTime = Time.time;
                player1.GetComponent<PlayerLogic>().enableControls();
                player2.GetComponent<PlayerLogic>().enableControls();
            }

            beginGame = true;
        }
    }

    public void activateResistanceSlider(Transform player)
    {
        if (player.gameObject.tag == "Player1")
        {
            resistanceSlider1.gameObject.SetActive(true);

            if (!seenResistancePrompt1)
            {
                //seenResistancePrompt1 = true;
                resistancePrompt1.SetActive(true);

            }
            

        } else if (player.gameObject.tag == "Player2")
        {
            resistanceSlider2.gameObject.SetActive(true);

            if (!seenResistancePrompt2)
            {
                //seenResistancePrompt2 = true;
                resistancePrompt2.SetActive(true);

            }
            
        }
    }

    public void deactivateResistanceSlider(Transform player)
    {
        if (player.gameObject.tag == "Player1")
        {
            resistanceSlider1.gameObject.SetActive(false);

            resistancePrompt1.SetActive(false);

        }
        else if (player.gameObject.tag == "Player2")
        {
            resistanceSlider2.gameObject.SetActive(false);
            resistancePrompt2.SetActive(false);
        }
    }

    public Transform getPlayer(int playerNum)
    {
        if (playerNum == 1)
        {
            return this.player1;
        }

        if (playerNum == 2)
        {
            return this.player2;
        }

        return null;
    }

    public bool isGameOver()
    {
        return this.gameOver;
    }

    public float getTimeRemaining()
    {
        return this.remainingTime;
    }

    public bool isGameStart()
    {
        return this.beginGame;
    }

}

