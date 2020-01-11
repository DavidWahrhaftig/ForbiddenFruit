using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Transform[] allowedPlayers;
    [SerializeField] AudioClip openSound, closeSound;
    [SerializeField] float waitToCloseDuration = 1.5f;
    private Animator animator;
    private AudioSource audioSource;

    bool isOpen = false;


    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {    
        if (FindObjectOfType<GameManager>().isGameStart() && !isOpen)
        {
            open();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (isPermitted(other.gameObject))
        {
            open();
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        /*if (isPermitted(other.gameObject))
        {
            Invoke("close", waitToCloseDuration);
        }
        */
    }


    bool isPermitted(GameObject player)
    {
        for (int i = 0; i < allowedPlayers.Length; i++)
        {
            if (player.tag == allowedPlayers[i].gameObject.tag)
            {
                return true;
            }
        }

        return false;
    }


    void open()
    {
        animator.SetTrigger("open");
        audioSource.Stop();
        audioSource.PlayOneShot(openSound);
        isOpen = true;
    }

    void close()
    {
        animator.SetTrigger("close");
        audioSource.PlayOneShot(closeSound);
        isOpen = false;
    }

    public bool getIsOpen()
    {
        return isOpen;
    }
}
