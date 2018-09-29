using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glow : MonoBehaviour {

    public float delay;
    public float cycle;
    [Range(0,1)]
    public float onTime;


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float cycleTime = (Time.time + delay) % cycle;

        GetComponent<Renderer>().enabled = cycleTime < (onTime * cycle);
	}
}
