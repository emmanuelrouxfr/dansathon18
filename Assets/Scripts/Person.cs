using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {

    [Header("Prefabs")]
    public GameObject flechePrefab;
    public GameObject pointPrefab;

    public Glow fleche;
    public Glow[] points;

    [Header("Setup")]
    public int numPoints;
    public float cycleTime;
    [Range(0, 1)]
    public float onTime;

	// Use this for initialization
	void Start () {
        fleche = Instantiate(flechePrefab).GetComponent<Glow>();
        fleche.transform.parent = transform;
        points = new Glow[numPoints];
        for(int i=0;i<numPoints;i++)
        {
            points[i] = Instantiate(pointPrefab).GetComponent<Glow>();
            points[i].transform.localPosition = new Vector3(i * 1f, 0f, 0f);
            points[i].transform.parent = transform;
        }
	}

    // Update is called once per frame
    void Update()
    {
        fleche.transform.parent = transform;
        for (int i = 0; i < numPoints; i++)
        {
            points[i].delay = (i * 1f / numPoints) * cycleTime;
            points[i].cycle = cycleTime;
            points[i].onTime = onTime;

            points[i].transform.localPosition = new Vector3(i * 1f, 0f, 0f);
            points[i].transform.parent = transform;
        }

        fleche.delay = 1;
        fleche.cycle = cycleTime;
        fleche.onTime = onTime;

    }

        Vector3 getPosInPath(float progress)
    {
        return new Vector3(); // to fill
    }
}
