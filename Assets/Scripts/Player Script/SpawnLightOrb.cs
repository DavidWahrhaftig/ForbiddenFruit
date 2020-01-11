using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
public class SpawnLightOrb : MonoBehaviour
{
    public AudioClip shotSound;
    
    public GameObject firePoint;
    public AudioSource ShootingSoundAudioSource;
    public List<GameObject> vfx = new List<GameObject>();
    private GameObject effectToSpawn;

    // Magic ORB Meter
    [Range(0,100)]
    public float magicCharge = 100f; // start at 100 percent
    [Range(0,100)]
    public float magicShotPower = 20f; // percent usage of shot
    public float reloadSpeed = 10f; //  the time it takes to load the charge from 0% to 100%

    //private AudioSource audioSource;
    private PlayerLogic playerLogic;
    private PlayerController playerController;
    private Rewired.Player gamePadController;

    private bool notShooting = true;

    // Start is called before the first frame update
    void Start()
    {
        effectToSpawn = vfx[0];
        playerLogic = GetComponent<PlayerLogic>();
        playerController = GetComponent<PlayerController>();

        //audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        gamePadController = playerController.getGamePadController(); // have to reference in Update method, else gamePadController is null

        bool shootButton = gamePadController.GetButton("Shoot");


        if (magicCharge >= magicShotPower)
        {
            if (shootButton && notShooting && !playerLogic.isGlowing() && !playerLogic.isDisabled()) // only shoot when there is a charge and the player is not glowing
            {
                //audioSource.PlayOneShot(shotSound);
                //CreateEffect();
                //magicCharge -= magicShotPower; // decrease charge
                GetComponent<Animator>().SetTrigger("isShooting");
                notShooting = false;
            }
        }

        if (magicCharge < 100)
        {
            magicCharge += Time.deltaTime / reloadSpeed * 100;
        }
    }

    void CreateEffect()
    {
        GameObject visualEffect;

        if (firePoint != null)
        {
            visualEffect = Instantiate(effectToSpawn, firePoint.transform.position,
                Quaternion.identity);
            visualEffect.transform.rotation = firePoint.transform.rotation;

            // Scale Light Orb
            //visualEffect.transform.localScale = new Vector3(2f, 2f, 2f);
        }
        else
        {
            Debug.Log("Null Fire Point");
        }
    }

    public void shoot(float delay)
    {
        magicCharge -= magicShotPower; // decrease charge

        if (magicCharge <= Mathf.Epsilon)
        {
            magicCharge = 0f;
        }

        Invoke("shootLater", delay);
        
    }

    void shootLater()
    {
        //audioSource.PlayOneShot(shotSound);
        ShootingSoundAudioSource.PlayOneShot(shotSound);
        CreateEffect();
        notShooting = true;
        
    }

    public bool isShooting()
    {
        return !notShooting;
    }
}
