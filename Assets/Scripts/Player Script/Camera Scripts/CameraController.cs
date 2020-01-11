using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 20f;

    [SerializeField] float upperBound = 0.162f;
    [SerializeField] float lowerBound = -0.0133f;


    // Game Pad related fields
    private Rewired.Player gamePadController;
    private float lookVertical;
    private bool cameraFlip;
    float rotationFactor;
    bool isFlipped = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        gamePadController = GetComponentInParent<PlayerController>().getGamePadController();

        lookVertical = gamePadController.GetAxis("Look Vertical");
        cameraFlip = gamePadController.GetButton("Camera Flip");
        rotationFactor = rotationSpeed * lookVertical * Time.deltaTime;

        //Debug.Log("camera.rotaiton.x = " + transform.localRotation.x);

        if (!isFlipped) { 
            if (overUpperBound() || underLowerBound() || transform.localRotation.x < upperBound && transform.localRotation.x > lowerBound)
            {
                transform.Rotate(rotationFactor, 0, 0, Space.Self); // camera vertical rotation
            }
        }


        if (cameraFlip) // pressed on flip button
        {
            
            if (!isFlipped)
            {
                isFlipped = true;
                // move camera in front/back of player
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z * -1f);

                // flip camera
                transform.Rotate(0f, 180, 0f, Space.World);
            }
            
            
            // inverse the horizontal movment and horizontal rotation in PlayerController.cs
        }
        else // rlease of flip button
        {
            if (isFlipped)
            {
                isFlipped = false;
                // move camera in front/back of player
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z * -1f);

                // flip camera
                transform.Rotate(0f, -180, 0f, Space.World);
            }
        }
    }

    bool overUpperBound()
    {
        return transform.localRotation.x >= upperBound && rotationFactor <= Mathf.Epsilon;
    }

    bool underLowerBound()
    {
        return transform.localRotation.x <= lowerBound && rotationFactor >= Mathf.Epsilon;
    }

    public void flipCamera()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z * -1f);

        // flip camera
        transform.Rotate(0f, 180, 0f, Space.World);
    }
}
