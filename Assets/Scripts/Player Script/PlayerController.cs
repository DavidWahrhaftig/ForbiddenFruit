using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;
public class PlayerController : MonoBehaviour
{
    [Header("Game Pad Controller Settings")]
    public int gamePadID;
    [SerializeField] Rewired.Player gamePadController;

    [Header("Speed Settings")]
    public float walkingSpeed;
    public float movingSpeed;
    public float rotationSpeed;
    public float stamina;
    public float centerToBaseSpeed;

    [Header("Jump Settings")]
    public float jumpForce;
    public ForceMode forceType;
    public bool grounded = true; // flag

    [Header("Sound Settings")]
    public AudioSource audioSource;
    public AudioClip walkingSound;
    public AudioClip jumpSound;

    /*
    public int motorIndex = 0;
    [Range(0,1)]
    public float motorLevel = 0.5f;
    public float duration = 2f;
    */

    #region Private Fields
    
    private float defaultY;
    private Rigidbody body;
    private GameManager gameManager;
    private PlayerLogic playerLogic;
    private Animator animator;
    bool isLevitating = false;
    bool isCollectingFruit = false;
    bool isUnderSpell = false;
    #endregion

    #region Controller Inputs
    float moveVertical;
    float moveHorizontal;
    float rotateHorizontal;
    bool jumpButton;
    bool runButton;
    bool isCameraFlipped;
    int flip = 1;
    bool isShooting;
    #endregion

    Vector3 movement;

    
    // Start is called before the first frame update
    void Start()
    {
        #region Components and GameObjects Referencing
        gamePadController = Rewired.ReInput.players.GetPlayer(gamePadID);
        gameManager = FindObjectOfType<GameManager>();
        playerLogic = GetComponent<PlayerLogic>();
        body = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();
        #endregion 
        movingSpeed = walkingSpeed;

        
        //gamePadController.SetVibration(motorIndex, motorLevel, duration, true);
    }

    /**
     * Update runs at the same frequency as the game's framerate. 
     * If you're getting 100 FPS, then Update() runs 100 times per second. 
    **/
    private void Update()
    {
        /*
        moveVertical = gamePadController.GetAxis("Move Vertical");
        moveHorizontal = gamePadController.GetAxis("Move Horizontal");
        rotateHorizontal = gamePadController.GetAxis("Rotate");
        jumpButton = gamePadController.GetButtonDown("Jump");
        runButton = gamePadController.GetButton("Run");
        isCameraFlipped = gamePadController.GetButtonDown("Camera Flip");
        isShooting = gamePadController.GetButton("Shoot");
        //bool centerToBase = gamePadController.GetButtonDown("CenterToBase");


        movement = transform.forward * moveVertical + transform.right * moveHorizontal;
        */
    }

    private void FixedUpdate()
    {
        moveVertical = gamePadController.GetAxis("Move Vertical");
        moveHorizontal = gamePadController.GetAxis("Move Horizontal");
        rotateHorizontal = gamePadController.GetAxis("Rotate");
        jumpButton = gamePadController.GetButtonDown("Jump");
        runButton = gamePadController.GetButton("Run");
        isCameraFlipped = gamePadController.GetButtonDown("Camera Flip");
        isShooting = gamePadController.GetButton("Shoot");
        //bool centerToBase = gamePadController.GetButtonDown("CenterToBase");


        movement = transform.forward * moveVertical + transform.right * moveHorizontal;

        if (isLevitating)
        {
            levitate();
        }

        if (!playerLogic.isDisabled())
        {

            // horizontal rotation movement
            //transform.Rotate(0, rotationSpeed * rotateHorizontal * Time.fixedDeltaTime, 0);
            // forward/backward movement

            rotatePlayer();

            if (!isShooting && !isCollectingFruit && !isUnderSpell)
            {
                body.MovePosition(transform.position + movement * movingSpeed * Time.deltaTime);

                if (Mathf.Abs(moveVertical) > 0.8f || Mathf.Abs(moveHorizontal) > 0.8f) // Running
                {
                    animator.SetBool("isRunning", true);
                    animator.SetBool("isIdle", false);
                    animator.SetBool("isWalking", false);

                    playSound(walkingSound);
                    movingSpeed = walkingSpeed * 2;
                    stamina -= 0.1f;
                }
                #region Idle & Walk Animation Transitions
                else if (Mathf.Abs(moveVertical) > Mathf.Epsilon && grounded) // Walking
                {
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isIdle", false);

                    animator.SetBool("isRunning", false);
                    movingSpeed = walkingSpeed;
                    stamina += 0.01f;
                }
                else // Idle
                {
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isIdle", true);

                    animator.SetBool("isRunning", false);
                    movingSpeed = walkingSpeed;
                    //stamina += 0.01f;
                }
                #endregion


                if (jumpButton && !GetComponent<PlayerLogic>().isGlowing())
                {
                    if (grounded)
                    {
                        Jump(jumpForce, forceType);
                    }
                }

            } else
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isIdle", true);
                animator.SetBool("isWalking", false);
            }
        }
    }

    private void rotatePlayer()
    {
        float turn = rotateHorizontal * rotationSpeed * Time.deltaTime;

        //Debug.Log("yRotation " + turn);
        // Make this into a rotation in the y axis.
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        body.MoveRotation(body.rotation * turnRotation);
    }

    void Jump(float force, ForceMode type)
    {
        playJumpingSound();
        setGrounded(false);
        //playSound(jumpSound);
        body.AddForce(transform.up * force, type);
        animator.SetBool("isJumping", true);
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }

    public void levitate()
    {
        setGravity(false);
        setGrounded(false);
        //body.AddForce(transform.up * 3, ForceMode.Force);
        if (transform.position.y < 2f)
        {
            body.MovePosition(transform.position + transform.up * 0.15f * Time.deltaTime);
        }
    }
    
    public void setGrounded(bool b)
    {
        grounded = b;
    }

    public void playSound(AudioClip audio)
    {
        if (!audioSource.isPlaying) // so it doesn't layer
        {
            audioSource.PlayOneShot(audio);
        }
    }

    public void playJumpingSound()
    {
        if (grounded)
        {
            //audioSource.Stop();
            audioSource.PlayOneShot(jumpSound);
        }
    }

    public Rewired.Player getGamePadController()
    {
        return this.gamePadController;
    }


    public void setGravity(bool b)
    {
        body.useGravity = b;
    }

    public void won()
    {
        playerLogic.disableControls();
        animator.SetBool("isWinner", true);
        animator.SetBool("isIdle", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isJumping", false);

    }

    public void lose()
    {
        playerLogic.disableControls();
        animator.SetBool("isLoser", true);
        animator.SetBool("isIdle", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isJumping", false);

    }

    public void setIsLevitating(bool b)
    {
        isLevitating = b;
    }

    public void setIsCollectingFruit(bool b)
    {
        isCollectingFruit = b;
    }

    public void setIsUnderSpell(bool b)
    {
        isUnderSpell = b;
    }

    public bool getIsUnderSpell()
    {
        return isUnderSpell;
    }


}

