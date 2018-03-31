﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    [SerializeField]
    float rcsThrust = 100f;

    [SerializeField]
    float mainThrust = 100f;

    [SerializeField]
    AudioClip mainEngine;
    [SerializeField]
    AudioClip levelLoad;
    [SerializeField]
    AudioClip dying;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    void RespondToThrustInput()
    {

        if (Input.GetKey(KeyCode.Space)) 
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
        }
    }

    void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    void RespondToRotateInput()
    {

        rigidBody.freezeRotation = true; //Take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //Resume physics control of rotation
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    void StartSuccessSequence()
    {
        print("Finished");
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(levelLoad);
        Invoke("LoadNextScene", 1f);
    }

    void StartDeathSequence()
    {
        print("Dead");
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(dying);
        Invoke("LoadFirstScene", 1f);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }
}
