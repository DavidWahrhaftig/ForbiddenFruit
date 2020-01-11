using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchOscillator : MonoBehaviour
{
    [SerializeField] float heightDisplacement = 0.3f;
    [SerializeField] float period = 2f;
    public bool isOscillating = true;
    public Transform oscillatorObject;

    float movementFactor;
    Vector3 movementVector;
    Vector3 startingPos; // must be stored for absolute movement
    // Start is called before the first frame update
    void Start()
    {
        startingPos = oscillatorObject.position;
        movementVector = new Vector3(0f, heightDisplacement, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }

        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2f; // about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        if (isOscillating)
        {
            oscillatorObject.position = startingPos + offset;
        }
    }
}
