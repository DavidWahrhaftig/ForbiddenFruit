﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAreaForFruits : MonoBehaviour
{
    public Color GizmosColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);
    public int limit = 5;
    public int fruitsInside = 0;

    void OnDrawGizmos()
    {
        Gizmos.color = GizmosColor;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

    public Vector3 getRandomPosition()
    {

        Vector3 origin = transform.position;
        Vector3 range = transform.localScale / 2.0f;
        Vector3 randomRange = new Vector3(Random.Range(-range.x, range.x),
                                          Random.Range(-range.y, range.y),
                                          Random.Range(-range.z, range.z));
        Vector3 randomCoordinate = origin + randomRange;

        return randomCoordinate;
    }

    public bool hasSpace()
    {
        //Debug.LogWarning("fruitsInside < limit  = " + fruitsInside + " < " + limit);
        return fruitsInside < limit;
    }

    public void incramentFruitCounter()
    {
        fruitsInside++;        
    }

    public void decreaseFruitsInside()
    {
        fruitsInside--;
    }

    public int getLimit()
    {
        return limit;
    }

}
