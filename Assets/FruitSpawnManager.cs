using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawnManager : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject fruit;
    public float radius;
    public Collider[] colliders;
    //public ArrayList<> collidersNoTrigers;
    public SpawnAreaForFruits previousArea;
    private SpawnAreaForFruits[] spawnAreas;
    void Start()
    {
        fruit = this.gameObject;
        spawnAreas = FindObjectsOfType<SpawnAreaForFruits>();
        Debug.LogWarning("spawnAreas.Length = " + spawnAreas.Length);
    }

    public void SpawnFruit()
    {
        Vector3 spawnPos = new Vector3(0, 0, 0);
        bool canSpawnHere = false;
        bool foundAvailableSpace;
        int safetyNet = 0;
        int randomIndex = 0;
        //randomIndex = Random.Range(0, spawnAreas.Length);

        while (!canSpawnHere)
        {
            //float spawnPosX = Random.Range(fruit.transform.position.x - 4f, fruit.transform.position.x + 4f);
            //float spawnPosY = Random.Range(fruit.transform.position.y, fruit.transform.position.y);
            //float spawnPosZ = Random.Range(fruit.transform.position.z - 4f, fruit.transform.position.z + 4f);

            //spawnPos = new Vector3(spawnPosX, spawnPosY, spawnPosZ);

            foundAvailableSpace = false;

            while(!foundAvailableSpace)
            {
                randomIndex = Random.Range(0, spawnAreas.Length);
                foundAvailableSpace = spawnAreas[randomIndex].hasSpace();
            }

            spawnPos = spawnAreas[randomIndex].getRandomPosition();

            canSpawnHere = PreventSpawnOverLap(spawnPos);
            
            if (canSpawnHere)
            {
                transform.position = spawnPos;
                Debug.LogWarning("randomIndex " + randomIndex);
                spawnAreas[randomIndex].incramentFruitCounter(previousArea);
                previousArea = spawnAreas[randomIndex];
                break;
            }

            safetyNet++;

            if (safetyNet > 50)
            {
                Debug.Log("Too many attempts");
                break;
               
            }

        }
    }

    bool PreventSpawnOverLap(Vector3 spawnPos)
    {
        colliders = Physics.OverlapSphere(transform.position, radius);
        int nonTriggers = 0;

        for (int i = 0; i <colliders.Length; i++)
        {
            if (!colliders[i].isTrigger)
            {
                nonTriggers++;
                Vector3 centerPoint = colliders[i].bounds.center;
                float width = colliders[i].bounds.extents.x;
                float height = colliders[i].bounds.extents.y;
                float depth = colliders[i].bounds.extents.z;

                float leftExtend = centerPoint.x - width;
                float rightExtend = centerPoint.x + width;
                float lowerExtend = centerPoint.y - height;
                float upperExtend = centerPoint.y + height;
                float backExtend = centerPoint.z - depth;
                float forwardExtend = centerPoint.z + depth;

                if (spawnPos.x >= leftExtend && spawnPos.x <= rightExtend)
                {
                    if (spawnPos.y >= lowerExtend && spawnPos.y <= upperExtend)
                    {
                        if (spawnPos.z >= backExtend && spawnPos.z <= forwardExtend)
                        {
                            //Debug.LogWarning("Non triggers = " + nonTriggers);
                            return false;
                        }
                    }
                }
            }
            
        }

        //Debug.LogWarning("Non triggers = " + nonTriggers);
        return true;
    }
}
