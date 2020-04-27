using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitCreater : MonoBehaviour
{
    // Start is called before the first frame update

    SpawnAreaForFruits[] spawnAreas;
    [SerializeField] GameObject fruitTemplateObject;

    public bool manuallySetFruit = false;
    public int numFruits;

    void Start()
    {
        spawnAreas = FindObjectsOfType<SpawnAreaForFruits>();
        //Debug.LogWarning("spawn areas for fruit creation : " + spawnAreas.Length);
        distributeFruits();    
    }


    private void distributeFruits()
    {
        int fruitsCreated = 0;
        int totalLimit = 0;
        
        for (int i = 0; i < spawnAreas.Length; i++)
        {
            totalLimit += spawnAreas[i].getLimit();

        }

        if (manuallySetFruit)
        {
            // only allow maximum limit in spawn areas
            if (numFruits > totalLimit)
            {
                numFruits = totalLimit;
            }
        }
        else
        {
            numFruits = totalLimit - 10; // to create available space  for fruits to switch areas
        }

        while (fruitsCreated < numFruits)
        {
            
            GameObject newFruit = createFruit();
            //Debug.LogWarning("About to spawn new fruit");
            newFruit.GetComponent<FruitSpawnManager>().setSpawnAreas(spawnAreas);
            newFruit.GetComponent<FruitSpawnManager>().SpawnFruit();
            fruitsCreated++;
        }
    }

    private GameObject createFruit()
    {
        GameObject newFruit; 

        if (fruitTemplateObject != null)
        {
            newFruit = Instantiate(fruitTemplateObject, transform.position,
                Quaternion.identity);
            newFruit.transform.parent = gameObject.transform; // set as child

            return newFruit;
            //fruitToLose.transform.rotation = transform.rotation;
            //fruitToLose.GetComponent<FruitLossProp>().setPlayer(gameObject);

            //fruitToLose.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)));
            //fruitToLose.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-0.5f, 0.5f), 0f, 0f));
        }
        
        Debug.Log("Null Fruit to Create");
        return null;    
    }


}
