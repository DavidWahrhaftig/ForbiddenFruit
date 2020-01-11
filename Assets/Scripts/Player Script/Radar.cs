using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform enemyPlayer;
    [SerializeField] Transform arrow;
    [SerializeField] Transform direction;

    float arrowAngle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direction.LookAt(enemyPlayer); 
        arrowAngle = direction.eulerAngles.y;
        arrow.eulerAngles = new Vector3(arrow.eulerAngles.x, arrowAngle, arrow.eulerAngles.z);
    }
}
