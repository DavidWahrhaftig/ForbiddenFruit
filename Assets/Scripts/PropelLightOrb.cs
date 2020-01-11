using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropelLightOrb : MonoBehaviour
{
    public float speed;
    public float fireRate;
    public GameObject impactPrefab;
    public float orbHealth;
    public string enemyplayerTag;
    private GameObject enemyPlayer;
    public bool honing;
    private float honingRotationSpeed = Mathf.Epsilon;


    // Start is called before the first frame update
    void Start()
    {
        enemyPlayer = GameObject.FindGameObjectWithTag(enemyplayerTag);
    }

    // Update is called once per frame
    void Update()
    {
        if (speed != 0)
        {
            if (honing)
            {
                Quaternion honingRotation = Quaternion.LookRotation(enemyPlayer.transform.position - transform.position);

                transform.rotation = Quaternion.Lerp(transform.rotation, honingRotation, Time.deltaTime - honingRotationSpeed);
            }

            else
            {
                // Keep the orb rotated to a fixed x axis.
                Vector3 eulers = transform.eulerAngles;
                eulers.x = 0;
                transform.eulerAngles = eulers;

                // Keep the orb below a set y-value
                if (transform.position.y > 2f)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
                }
            }
            

            // Propel the orb forward
            transform.position += transform.forward * (speed * Time.deltaTime);

            // Decrease orb health over time
            orbHealth -= 0.1f;

            // Destroy orb if health is below 0
            if(orbHealth <= 0)
            {
                DestroyOrb(transform.position, transform.rotation);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Destroy the orb if it hits a player
        if (collision.gameObject.tag == enemyplayerTag)
        {
            speed = 0;

            ContactPoint contact = collision.contacts[0];
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;

            DestroyOrb(pos, rotation);

        }
        /*
        else if (collision.transform.tag == "SpecialFruit")
        {
            speed = 0;

            ContactPoint contact = collision.contacts[0];
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;

            collision.gameObject.GetComponent<SpecialFruit>().putSpellOnFruit(gameObject.tag);

            DestroyOrb(pos, rotation);
        }
        */

        // Ricochet the light orb off any other surface
        else
        {
            Vector3 reflectDir = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
            float rotation = 90 - (Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg);
            transform.eulerAngles = new Vector3(0, rotation, 0);
            honing = false;

        }

    }

    private void DestroyOrb(Vector3 pos, Quaternion rotation)
    {
        if (impactPrefab != null)
        {
            var impactVFX = Instantiate(impactPrefab, pos, rotation);
            var psHit = impactVFX.GetComponent<ParticleSystem>();

            if (psHit != null)
            {
                Destroy(impactVFX, psHit.main.duration);
            }

            else
            {
                var psChild = impactVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(impactVFX, psChild.main.duration);
            }
        }

        Destroy(gameObject);
    }

    public void contactFruit()
    {
        //speed = 0;

        //ContactPoint contact = collision.contacts[0];
        //Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        //Vector3 pos = contact.point;

        //DestroyOrb(pos, rotation);
        DestroyOrb(transform.position, transform.rotation);
    }

    public void startHoning()
    {
        honing = true;
    }
}

