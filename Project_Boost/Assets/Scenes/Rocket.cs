using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

   void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // print("Thrusting");
            rigidBody.AddRelativeForce(Vector3.up);
            if (!audioSource.isPlaying)  //so it doesn't layer
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
        


        if(Input.GetKey(KeyCode.D)) //separate statement- can simulaneous directions
        {
            print("Rotate Left");
            transform.Rotate(Vector3.forward);
        }
 
        if(Input.GetKey(KeyCode.F))
        {
            print("Rotate Right");
            transform.Rotate(Vector3.back);
        }
    }
}
