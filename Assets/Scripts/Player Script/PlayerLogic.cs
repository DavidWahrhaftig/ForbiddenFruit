using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] Transform playerBase;

    public GameObject enemyProjectile;

    public AudioClip hitSound, caughtSound;

    public TextMeshProUGUI counterUI;


    [SerializeField] SkinnedMeshRenderer playerSkin;
    [SerializeField] Material glowMaterial;

    private AudioSource audioSource;

    private Rigidbody selfRigidbody;
    public int fruitCounter = 0;

    private Material ogMaterial;

    //Flags
    public bool glowing = false; // for witch and hidden ability
    public bool disabled = false; // controls 
    public bool hidden = false; 
    public bool caught = false; 
    public bool canBeChased = true;
    private Vector3 originalRotation;
    private PlayerController playerController;
    private Animator animator;
    private string enemyTag;

    [Range(0,1)]
    public float materialTransition = 0f;

    [SerializeField] GameObject fruitToLoseObject;

    [SerializeField] GameObject radar;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        selfRigidbody = GetComponent<Rigidbody>();

        ogMaterial = playerSkin.material;

        playerController = GetComponent<PlayerController>();

        originalRotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);

        if(gameObject.tag == "Player1")
        {
            enemyTag = "Player2";
        }
        else if (gameObject.tag == "Player2")
        {
            enemyTag = "Player1";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (glowing)
        {
            if (hidden) { stopChasingMe(); }
        }

        playerSkin.material.Lerp(ogMaterial, glowMaterial, materialTransition);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Fruit")
        {
            //playFruitSound();
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());

            Debug.Log("FruitCounter: " + fruitCounter);
        }

        if (collision.transform.tag == "Ground")
        {
            if (!playerController.grounded)
            {
                playerController.setGrounded(true);
                transform.GetComponent<Animator>().SetBool("isJumping", false);
            }

        }

        if (collision.gameObject.name == enemyProjectile.name + "(Clone)")
        {
            playSound(hitSound);

            // 

            if (getCanBeChased() && 
                !GameObject.FindGameObjectWithTag(enemyTag).GetComponent<PlayerLogic>().isGlowing() && 
                !GameObject.FindGameObjectWithTag(enemyTag).GetComponent<PlayerLogic>().isCaught())
            {
                chaseMe();
            }
            
        }
    }


    public void changeMaterial(Material material)
    {
        //var playerRend = gameObject.GetComponent<Renderer>();
        playerSkin.material = material;
    }

    public void chaseMe()
    {
        WitchLogic witchLogic = FindObjectOfType<GameManager>().getWitch().GetComponent<WitchLogic>();
        witchLogic.chase(transform);

        changeMaterial(glowMaterial);
        setGlowing(true); // disable this player from shooting in SpawnLightOrb.cs 

    }

    public void stopChasingMe()
    {
        WitchLogic witchLogic = FindObjectOfType<GameManager>().getWitch().GetComponent<WitchLogic>();
        witchLogic.stopChasing();

        changeMaterial(ogMaterial);
        setGlowing(false);
        setHidden(false);
    }

    /*
    public void spawn()
    {
        playerController.getAnimator().SetBool("isGettingUp", true);
    }
    */

    public bool isHidden()
    {
        return hidden;
    }

    public void setHidden(bool b)
    {
        hidden = b;
    }

    public bool isGlowing()
    {
        return glowing;
    }

    public void setGlowing(bool b)
    {
        glowing = b;
    }

    public int getFruitCounter()
    {
        return fruitCounter;
    }


    public void incrementFruitCounter()
    {
        fruitCounter += 1;
        counterUI.GetComponent<Animator>().SetTrigger("fruitGain"); // do gain animation of fruit counter

    }

    public void loseFruits(int numfruits, bool vibrate)
    {
        int fruitToLose = numfruits;

        // do nothing
        if (numfruits == 0)
        {
            return;
        }

        // when we are losing more fruits than we have
        if (numfruits > fruitCounter)
        {
            fruitToLose = fruitCounter;
        }

        // update the fruitCoutner
        fruitCounter -= fruitToLose;
        // animate the fruitCounter UI
        counterUI.GetComponent<Animator>().SetTrigger("fruitLoss"); // do lose animation of fruit counter

        // create the fruitToLose amount of fruit props
        for (int i = 0; i < fruitToLose; i++)
        {
            CreateFruitToLose();
        }

        if(vibrate)
        {
            playerController.getGamePadController().SetVibration(0, 0.5f, 0.6f, true);
        }
    }

    public void playSound(AudioClip audio)
    {
        if (!audioSource.isPlaying) // so it doesn't layer
        {
            audioSource.PlayOneShot(audio);
        }
    }


    public void gotCaught()
    {
        caught = true;
        disabled = true;
        animator.SetBool("isIdle", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);
    }

    public void disableControls()
    {
        disabled = true;
    }

    public void enableControls()
    {
        caught = false;
        disabled = false;
        //stopChasingMe(); // moved to PlayerRespawnBehaviour.cs
    }

    public bool isCaught()
    {
        return this.caught;
    }

    public void setCaught(bool b)
    {
        this.caught = b;
    }

    public bool isDisabled()
    {
        return this.disabled;
    }

    public bool getCanBeChased()
    {
        return this.canBeChased;
    }

    public void setCanBeChased(bool b)
    {
        this.canBeChased = b;
    } 

    public Transform getPlayerBase()
    {
        return this.playerBase;
    }

    public Quaternion getOriginalRotation()
    {
        return Quaternion.Euler(originalRotation.x, originalRotation.y, originalRotation.z);
    }


    void CreateFruitToLose()
    {
        GameObject fruitToLose;

        if (fruitToLoseObject != null)
        {
            fruitToLose = Instantiate(fruitToLoseObject, transform.position + new Vector3(0f, Random.Range(0.3f, 1f), 0f),
                Quaternion.identity);
            fruitToLose.transform.rotation = transform.rotation;
            fruitToLose.GetComponent<FruitLossProp>().setPlayer(gameObject);

            fruitToLose.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f,1f), 0f, Random.Range(-1f, 1f)));
            fruitToLose.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-0.5f, 0.5f), 0f, 0f));
        }
        else
        {
            Debug.Log("Null Fruit to Lose");
        }
    }

    public void setRadarOn(bool b)
    {
        if (caught)
        {
            radar.SetActive(false);
        } else
        {
            radar.SetActive(b);
        }
        
    }
}


