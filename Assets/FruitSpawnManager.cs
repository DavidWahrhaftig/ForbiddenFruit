using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawnManager : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject fruit;
    public float radius;
    public Collider[] colliders;
    public SpawnAreaForFruits currentArea;
    public SpawnAreaForFruits[] spawnAreas;
    private Vector3 originalPos;
    void Start()
    {
        fruit = this.gameObject;
        //spawnAreas = FindObjectsOfType<SpawnAreaForFruits>();  --------> made a setter method, otherwise null error occurs
        originalPos = transform.position;
        //Debug.LogWarning("spawnAreas.Length = " + spawnAreas.Length);
    }

    public void SpawnFruit()
    {
        Vector3 spawnPos = new Vector3(0, 0, 0);
        bool canSpawnHere = false;
        bool foundAvailableSpace;
        int safetyNet = 0;
        int randomIndex = 0;

        GetComponent<Oscillator>().isOscillating = false;

        if(currentArea)
        {
            currentArea.decreaseFruitsInside();
        }
        
        while (!canSpawnHere)
        {
            foundAvailableSpace = false;
            safetyNet++;

            if (! stillSpaceAvailable() || safetyNet > 50)
            {

                /*
                // fruit will spawn at the same location it is in. 
                
                if (currentArea)
                {
                    //currentArea.incramentFruitCounter(); // self incramentation
                    transform.position = originalPos;
                }
                */
                //Debug.LogWarning("Still SpaceAvailable  = " + stillSpaceAvailable());
                Debug.LogWarning("didn't move, same spot still");
                if (safetyNet > 50)
                {
                    Debug.Log("Too many attempts");
                }
                transform.position = originalPos;
                //GetComponent<Oscillator>().setStartingPos(;
                GetComponent<Oscillator>().setStartingPos(originalPos);
                GetComponent<Oscillator>().isOscillating = true;
                return;
            }


            while (!foundAvailableSpace)
            {
                randomIndex = UnityEngine.Random.Range(0, spawnAreas.Length);
                foundAvailableSpace = spawnAreas[randomIndex].hasSpace();   
            }

            spawnPos = spawnAreas[randomIndex].getRandomPosition();

            canSpawnHere = PreventSpawnOverLap(spawnPos);
            
            if (canSpawnHere)
            {
                transform.position = spawnPos;
                Debug.LogWarning("randomIndex " + randomIndex);
                currentArea = spawnAreas[randomIndex];
                currentArea.incramentFruitCounter();
                GetComponent<Oscillator>().setStartingPos(spawnPos);
                GetComponent<Oscillator>().isOscillating = true;
                break;
            }
        }

        
    }

    private bool stillSpaceAvailable()
    {
        Debug.LogWarning("SpawnAreas.Length = " + spawnAreas.Length);
        for (int i = 0; i < spawnAreas.Length; i ++)
        {
            if (spawnAreas[i].hasSpace())
            {
                return true;
            }
        }
        return false;
    }

    bool PreventSpawnOverLap(Vector3 spawnPos)
    {
        int numColliding = Physics.OverlapSphereNonAlloc(spawnPos, radius, colliders);
        
        //colliders = Physics.OverlapSphere(transform.position, radius);
        //colliders = Physics.OverlapSphere(spawnPos, radius);
        //int numColliding = Physics.OverlapSphereNonAlloc(spawnPos, radius, colliders);

        //int nonTriggers = 0;

        for (int i = 0; i < colliders.Length; i++)
        {

            // don't skip fruits or envrionemnt props colliders
            if (colliders[i].gameObject.tag == "SpecialFruit" || colliders[i].gameObject.tag == "Environment Prop" || colliders[i].gameObject.tag == "Collider Wall")
            {
                Debug.LogWarning("----colliding with a fruit or environment prop or collider ----");
                return false;

            }
        }

        //Debug.LogWarning("Non triggers = " + nonTriggers);
        return true;
    }


    public void setSpawnAreas(SpawnAreaForFruits[] areas)
    {
        spawnAreas = areas;
    }
}
