using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillating : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 0f, 0f);
    [SerializeField] float period = 6f;

    //todo remove from inspector later
    // [Range(0,1)] [SerializeField] 
    float movementFactor; //0 not moved, 1 fully moved

    Vector3 startingPos;
    
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period; // grows continuall from 0

        const float tau = Mathf.PI * 2f; //about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);


        movementFactor = rawSinWave / 2f + 0.5f; ;
        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
    }
}
