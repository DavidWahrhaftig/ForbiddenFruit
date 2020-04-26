using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFruit : MonoBehaviour
{
    // Start is called before the first frame update

    public bool respawnable = true;

    [SerializeField] GameObject neutralFruit;
    [SerializeField] GameObject redFruit;
    [SerializeField] GameObject blueFruit;

    [SerializeField] AudioClip[] fruitSounds;

    [SerializeField] float spellDuration = 5f; // how long a fruit will be under a spell before becoming neutral again
    [SerializeField] float respawnTime = 5f;
    [SerializeField] float collectTime = 0.1f;
    [SerializeField] float respawnDistance = 15f;

    GameObject currentActiveFruit;
    public AudioSource audioSource;
    private bool isUnderSpell = false;
    private bool isCollectable = true;


    private int fruitValue = 1;
    private FruitSpawnManager fruitSpawnManager;
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(0.9f, 1.1f);

        redFruit.SetActive(false);
        blueFruit.SetActive(false);

        currentActiveFruit = neutralFruit;
        fruitSpawnManager = GetComponent<FruitSpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (isCollectable)
        {
            if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
            {
                if (canPlyerTakeFruit(other.gameObject.tag))
                {
                    isCollectable = false;
                    isUnderSpell = false;

                    other.gameObject.GetComponent<PlayerLogic>().incrementFruitCounter(fruitValue);
                    playFruitSound();

                    //Invoke("setFruitInactive", collectTime);
                    setFruitInactive();

                    Invoke("respawnFruit", respawnTime);

                }

            }
            else if (other.gameObject.tag == "ProjectileRed" || other.gameObject.tag == "ProjectileBlue")
            {
                putSpellOnFruit(other.gameObject.tag);
                other.gameObject.GetComponent<PropelLightOrb>().contactFruit();
            }
        }
    }

    private void setFruitInactive()
    {
        currentActiveFruit.SetActive(false);
        //Invoke("respawnFruit", respawnTime);
    }

    private bool canPlyerTakeFruit(string playerTag)
    {
        GameObject enemyFruit = null;
        if (playerTag == "Player1")
        {
            enemyFruit = blueFruit;

        }
        else if (playerTag == "Player2")
        {
            enemyFruit = redFruit;
        }

        return currentActiveFruit != enemyFruit;
    }

    private void respawnFruit()
    {
        if (respawnable)
        {
            currentActiveFruit = neutralFruit;
            fruitValue = 1;
            StartCoroutine(respawnFruitCoRoutine());
        }
    }

    IEnumerator respawnFruitCoRoutine()
    {
        bool wait = true;
        if (fruitSpawnManager)
        {
            Debug.LogWarning("random respawns");
            fruitSpawnManager.SpawnFruit();
        } 
        
        while (wait)
        {
            if (minDistanceFromPlayers() > respawnDistance)
            {
                wait = false;

            }

            yield return null;


        }
        currentActiveFruit.SetActive(true);
        isCollectable = true;

        yield return null;


    }

    private void playFruitSound()
    {
        if (fruitSounds.Length != 0)
        {
            int randomIndex = Random.Range(0, fruitSounds.Length);
            audioSource.PlayOneShot(fruitSounds[randomIndex]);
        }
    }

    public void putSpellOnFruit(string projectileTag)
    {
        if (projectileTag == "ProjectileRed")
        {
            if (currentActiveFruit == neutralFruit)
            {
                currentActiveFruit.SetActive(false);
                fruitValue = 2;
                currentActiveFruit = redFruit;
            }
        }

        else if (projectileTag == "ProjectileBlue")
        {
            if (currentActiveFruit == neutralFruit)
            {
                currentActiveFruit.SetActive(false);
                fruitValue = 2;
                currentActiveFruit = blueFruit;
            }
        }

        if (!isUnderSpell)
        {
            isUnderSpell = true;
            currentActiveFruit.SetActive(true);
            Invoke("spellWearOff", spellDuration);
        }

    }

    private void spellWearOff()
    {
        if (isUnderSpell)
        {
            currentActiveFruit.SetActive(false);
            currentActiveFruit = neutralFruit;
            fruitValue = 1;
            currentActiveFruit.SetActive(true);

            isUnderSpell = false;
        }
    }

    private float minDistanceFromPlayers()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        float distancePlayer1 = Vector3.Distance(gameManager.getPlayer(1).position, transform.transform.position);
        float distancePlayer2 = Vector3.Distance(gameManager.getPlayer(2).position, transform.transform.position);

        return Mathf.Min(distancePlayer1, distancePlayer2);
    }

    public GameObject getCurrentActiveFruit()
    {
        return this.currentActiveFruit;
    }

}
