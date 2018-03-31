using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField]
    Vector3 movementVector = new Vector3(10f, 0f, 0f);

    [SerializeField]
    float period = 2f;

    Vector3 startingPos;

    float movementFactor;

    // Use this for initialization
    void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //set movement factor automatically
        if (period <= Mathf.Epsilon)
        {
            return;
        }
        float cycles = Time.time / period; //grows continually from 0

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + 0.5f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
	}
}
