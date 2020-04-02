using System;
using UnityEngine;
using UnityEngine.SceneManagement;

 

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;//RCS-- Reacion Control System
    [SerializeField] float levelLoadDelay = 2f;
   
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;


    enum State { Alive, Dying, Transcending}
    State state = State.Alive;

    bool collisionsDisabled = false;
    
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    
    void Update()                           // Update is called once per frame
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
            DebugMode();
        }
    }




    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionsDisabled == true) { return; } // ignore collisions for debugging

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //top off fuel
                break;
            case "Finish":
                successSequence();
                //parameterize time
                break;
            case "Fuel":
                print("Fuel up!");
                break;
            default:
                deathSequence();
                break;
        }

    }




    #region Atomic Code

    private void successSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        mainEngineParticles.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void deathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        mainEngineParticles.Stop();
        deathParticles.Play();
        audioSource.PlayOneShot(death);
        Invoke("ReloadCurrentScene", levelLoadDelay);
        //parameterize time
        //TODO -- kill player
    }
    private void ReloadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        RestartGame(currentSceneIndex);
        print(currentSceneIndex);
        SceneManager.LoadScene(nextSceneIndex);


    }

    private void RestartGame(int currentSceneIndex)
    {
        if (currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            LoadFirstLevel();
        }
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void ApplyThrust()
    {
        print("Thrusting");
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)  //so it doesn't layer
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    void RespondToRotateInput()
    {
        rigidBody.angularVelocity = Vector3.zero;    // remove rotation due to physics
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.D)) //separate statements- can simulaneous directions
        {
            
            print("Rotate Left");
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }

        if (Input.GetKey(KeyCode.F)) //separate statements- can simulaneous directions
        {
            print("Rotate Right");
            transform.Rotate(Vector3.back * rotationThisFrame);
        }
    }

    private void CollisionsToggled()
    {
        if (collisionsDisabled == false)
        {
            print("Collisions Disabled");
            collisionsDisabled = true;
        }
        else
        {
            print("Collisions Enabled");
            collisionsDisabled = false;
        }
    }

    private void DebugMode()
    {
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            CollisionsToggled(); // collisionsDisabled = !collisionsDisabled;

        }
    }

    #endregion

}
