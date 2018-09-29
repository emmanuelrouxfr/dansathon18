using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glow : MonoBehaviour {

    public Person LinkedPerson;

    public float delay;
    public float cycle;
    [Range(0,1)]
    public float onTime;


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float cycleTime = (Time.time + delay) % cycle;

        if (!LinkedPerson.IsOnMeetingPoint)
        {
            GetComponent<Renderer>().enabled = cycleTime < (onTime * cycle);
        }
        else
        {
            GetComponent<Renderer>().enabled = false;
        }

    }



}
