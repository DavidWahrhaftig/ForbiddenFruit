using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{


    private Rewired.Player gamePadController;

    private Animator camAnim;
    private bool shakeLeft, shakeRight;
    public float power = 0f;
    private float gain = 1f;

    public float resistanceMeter = 0;
    // Start is called before the first frame update

    public bool canShake;
    public bool enableShakeAnimation = true;
    void Start()
    {
        camAnim = GetComponent<Animator>();
        canShake = false;
        gamePadController = GetComponentInParent<PlayerController>().getGamePadController();
    }

    // Update is called once per frame
    void Update()
    {

        if (canShake)
        {
            shakeLeft = gamePadController.GetButtonDown("ShakeLeft");
            shakeRight = gamePadController.GetButtonDown("ShakeRight");

            if (shakeLeft)
            {
                camShakeLeft();

            }
            else if (shakeRight)
            {
                camShakeRight();
            }
        }
    }

    public void camShakeLeft()
    {
        if (enableShakeAnimation)
        {
            camAnim.SetTrigger("Shake Left");
        }
        
        power += gain;
    }

    public void camShakeRight()
    {
        if (enableShakeAnimation)
        {
            camAnim.SetTrigger("Shake Right");
        }
        
        power += gain;
    }

    public void setCanShake(bool b)
    {
        Debug.Log("setting can Shake");
        canShake = b;
    }
    public float getPower()
    {
        return power;
    }

    public void setPower(float value)
    {
        power = value;
    }

    public float getResistanceMeter()
    {
        return resistanceMeter;
    }

    public void setResistanceMeter(float value)
    {
        resistanceMeter = value;
    }
}
