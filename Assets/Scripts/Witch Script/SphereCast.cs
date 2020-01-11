using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCast : MonoBehaviour
{
    public GameObject currentHitObject; // for debugging
    public float sphereRadius; // for debugging
    
    public LayerMask layerMask;

    public float angle;
    public float currentHitDistance; // for debugging
    public float distance1;
    public Transform player;
   
    private Vector3 origin; // position of this gameObject
    private Vector3 direction; // the direction to shoot raycast
    //private Transform parent;
    private Oscillator oscillator; // access to gameObject's floating functionality
    private Light lightComponent;
    private Animator animator; 

    private WitchLogic witchLogic;

    // Start is called before the first frame update
    void Start()
    {
        witchLogic = GetComponentInParent<WitchLogic>();
        //oscillator = GetComponent<Oscillator>();
        lightComponent = GetComponent<Light>();
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            animator = transform.parent.transform.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        origin = transform.position;
        direction = Vector3.down;
        angle = lightComponent.spotAngle;
        RaycastHit hit1; // only used for sphere radius calculation
        RaycastHit hit2;
        Ray landingRay = new Ray(transform.position, Vector3.down);

        /*
          if (Physics.Raycast(from, dirToTarget, out hit, (1 << LayerMask.NameToLayer("Sight") | (1 << LayerMask.NameToLayer("OtherLayerMaskName"))))
         */

        if (Physics.Raycast(landingRay, out hit1))
        {
            Debug.DrawRay(transform.position, Vector3.down * hit1.distance);
            // calculate the spehre radius
            // opposite = adjacent * tan(angle)
            // radius = distance to collider * tan(spotlight angle / 2) --> using half the angle to obtain a right angle
            sphereRadius = hit1.distance * Mathf.Tan(Mathf.Deg2Rad * (this.lightComponent.spotAngle/2));
            distance1 = hit1.distance;

            if (Physics.SphereCast(origin, sphereRadius, direction, out hit2, hit1.distance, layerMask, QueryTriggerInteraction.UseGlobal))
            {
                currentHitObject = hit2.transform.gameObject;
                currentHitDistance = hit2.distance + sphereRadius; //to use sphere as a semisphere

                if (hit2.collider.tag == "Player1" || hit2.collider.tag == "Player2") 
                {
                    Debug.Log("Spherecast Got Player");

                    player = hit2.collider.transform;
                    
                    
                    if (canCapture(player) && !FindObjectOfType<GameManager>().isGameOver())
                    {
                        if (witchLogic.getTargetPlayer() != null && witchLogic.getTargetPlayer().gameObject != player.gameObject)
                        {
                            // caught a player while chasing another player

                            PlayerLogic otherPlayer = witchLogic.getTargetPlayer().GetComponent<PlayerLogic>();
                            otherPlayer.stopChasingMe();
                        }

                        player.GetComponent<PlayerLogic>().setCanBeChased(false);
                        witchLogic.setTargetPlayer(player);



                        animator.SetBool("isIdle", false);
                        animator.SetBool("isChasing", false);
                        animator.SetBool("isPatrolling", false);
                        animator.SetBool("isCapturing", true);
                    }
                }
            }           
        } 
    }

    private bool canCapture(Transform player)
    {
        // capture only if player can be chased
        if (player.GetComponent<PlayerLogic>().getCanBeChased())
        {
            if (witchLogic.getTargetPlayer() == null)
            {
                // no witch target, then player can be captured
                return true;
            }
            else if (!witchLogic.getTargetPlayer().GetComponent<PlayerLogic>().isCaught())
            {
                // no player is being captured
                return true;
            }
            
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        /*
         * Displays SphereCast Visually for Debuging purposes.
         * Can only be seen in the Scene window while game is running
         */

        Gizmos.color = Color.red;
        //Debug.DrawLine(origin, origin + direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }
}
