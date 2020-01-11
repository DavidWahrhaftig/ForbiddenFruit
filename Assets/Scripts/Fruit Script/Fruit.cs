using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{

    private AudioSource audioSource;
    [SerializeField] AudioClip[] fruitSounds;
    GameManager gameManager;
    [SerializeField] int respawnTime = 10;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); 
        //audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
        {
            other.GetComponent<PlayerLogic>().incrementFruitCounter();
            //gameManager.playFruitSound();
            gameObject.SetActive(false);

            Invoke("respawnFruit", respawnTime);

            //Destroy(gameObject);
        }
    }  

    private void respawnFruit()
    {
        gameObject.SetActive(true);
    }

    private void playFruitSound()
    {
        if (fruitSounds.Length != 0)
        {
            int randomIndex = Random.Range(0, fruitSounds.Length);
            audioSource.PlayOneShot(fruitSounds[randomIndex]);
        }
    }
}
