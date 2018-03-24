using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;

    float startVolume = 4f;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
    }

    IEnumerator VolumeFade(AudioSource _AudioSource, float _EndVolume, float _FadeLength)
    {
        float _StartTime = Time.time;
        while (!audioSource.isPlaying &&
               Time.time < _StartTime + _FadeLength)
        {
            _AudioSource.volume = startVolume + ((_EndVolume - startVolume) * ((Time.time - _StartTime) / _FadeLength));
            yield return null;
        }
        if (_EndVolume == 0) { _AudioSource.UnPause(); }
    }

    void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space)) // Thrusting
        {
            rigidBody.AddRelativeForce(Vector3.up);
            if (!audioSource.isPlaying)
            {
                audioSource.volume = startVolume;
                audioSource.UnPause();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                StartCoroutine(VolumeFade(audioSource, 10f, 0.1f));
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward);
        }
    }
   
}
