using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitLossProp : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float destoryTimer = 2.5f;
    [SerializeField] Collider collider;
    [SerializeField] AudioClip collectSound;
    //[SerializeField] AudioClip loseFruitSound;
    private AudioSource audioSource;

    private GameObject forbiddenPlayer;
    private bool collectable = false;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //Physics.IgnoreCollision(GetComponent<Collider>(), this.player.GetComponent<Collider>());
        Invoke("dissolveFruit", destoryTimer);
    }
    private void Update()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), this.forbiddenPlayer.GetComponent<Collider>());
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            audioSource.Play();
            collectable = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player1" || other.tag == "Player2")
        {
            if (other.tag != this.forbiddenPlayer.tag)
            {
                other.GetComponent<PlayerLogic>().incrementFruitCounter();
                other.GetComponent<AudioSource>().PlayOneShot(collectSound);
                Destroy(gameObject);
            }
        }        
    }

    void dissolveFruit()
    {
        collider.enabled = false;
        Invoke("destroyFruitProp", 2f);
    }

    void destroyFruitProp()
    {
        Destroy(gameObject);
    }

    public bool isCollectable()
    {
        return collectable;
    }

    public void setPlayer(GameObject player)
    {
        /**
         * this method is called from PlayerLogic in the loseFruit Method 
         **/

        this.forbiddenPlayer = player;
    }
}
