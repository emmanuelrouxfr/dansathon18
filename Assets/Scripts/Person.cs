using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;
using System;

public class Person : AugmentaPersonBehaviour
{

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

    [Header("Path")]
    public int splineIndex;

    [Range(0, 1)]
    public float flecheDist;

    [Header("Progress")]
    [Range(-1, 1)]
    public float currentProgress;
    float previousProgress;
    float progressSpeed;

    [Header("Meeting settings")]
    public float MeetingDuration;

    public bool CameFromLeft;
    public bool IsGoingToMeetingPoint = true;
    public bool IsOnMeetingPoint;

    // Use this for initialization
    void Start()
    {
        fleche = Instantiate(flechePrefab).GetComponentInChildren<Glow>();
        fleche.LinkedPerson = this;
        fleche.transform.parent.parent = transform;
        points = new Glow[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            points[i] = Instantiate(pointPrefab).GetComponent<Glow>();
            points[i].transform.parent = transform;
            points[i].LinkedPerson = this;
        }

        splineIndex = getClosestSplineIndex();

        if (splineIndex % 2 == 0)
            CameFromLeft = true;

       
    }

    // Update is called once per frame
    void Update()
    {
        currentProgress = getProgressForPosition();

        for (int i = 0; i < numPoints; i++)
        {
            points[i].delay = ((numPoints - 1 - i) * 1f / numPoints) * cycleTime;
            points[i].cycle = cycleTime;
            points[i].onTime = onTime;
            setTransformToPathRelative(points[i].transform, (i * 1f / numPoints) * flecheDist);
        }

        fleche.delay = 1;
        fleche.cycle = cycleTime;
        fleche.onTime = onTime;
        setTransformToPathRelative(fleche.transform, flecheDist);   
        if (!CameFromLeft)
            fleche.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));

        progressSpeed = progressSpeed - previousProgress; //*Time.deltaTime;
        previousProgress = currentProgress;

        OSCMessage msg = new OSCMessage("/person/" + pid, currentProgress);
        OSCMaster.sendMessage(msg, PathManager.instance.OutputIP, PathManager.instance.OutputPort);

        if (currentProgress >= 1.0f && !IsOnMeetingPoint) //Point on meeting point
        {
            OSCMessage msg2 = new OSCMessage("/person/" + pid + "/onMeetingPoint");
            OSCMaster.sendMessage(msg2, PathManager.instance.OutputIP, PathManager.instance.OutputPort);

            IsOnMeetingPoint = true;
            

            StartCoroutine(CallMethodAfterXSeconds(MeetingDuration, NextStep));
        }
    }

    public void NextStep()
    {
        OSCMessage msg = new OSCMessage("/person/" + pid + "/meetingPointLeft");
        OSCMaster.sendMessage(msg, PathManager.instance.OutputIP, PathManager.instance.OutputPort);

        IsGoingToMeetingPoint = false;
        IsOnMeetingPoint = false;

        splineIndex = getAssociatedPathTo(splineIndex);
    }

    public static IEnumerator CallMethodAfterXSeconds(float x, Action Callback)
    {
        var currentTime = 0.0f;

        while(currentTime <= x)
        {
            currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate(); 
        }

        if (Callback != null)
            Callback();
    }

    Vector3 getPosInPath(float progress)
    {
        if (PathManager.instance.Paths[splineIndex] == null) return transform.position;
        return PathManager.instance.Paths[splineIndex].GetPoint(CameFromLeft ? progress : (1.0f - progress));
    }

    Vector3 getDirectionOnPath(float progress)
    {
        if (PathManager.instance.Paths[splineIndex] == null) return transform.rotation.eulerAngles;
        return PathManager.instance.Paths[splineIndex].GetDirection(IsGoingToMeetingPoint ? progress : (1.0f - progress));
    }

    void setTransformToPathRelative(Transform t, float relativeProgress)
    {
        float p = Mathf.Clamp01(currentProgress + relativeProgress);
        t.position = getPosInPath(p);
        t.LookAt(t.position + getDirectionOnPath(p));
    }

    int getAssociatedPathTo(int index)
    {
        int result = -1;
        Debug.Log("LinkedPaths couunt : " + PathManager.instance.LinkedPaths.Count);
        foreach(var vector in PathManager.instance.LinkedPaths)
        {
            if ((int)vector.y == splineIndex)
            {
                result = (int)vector.x;
                break;
            }
            if ((int)vector.x == splineIndex)
            {
                Debug.Log("Found");
                result = (int)vector.y;
                break;
            }
        }

        return result;
    }

    int getClosestSplineIndex()
    {
        BezierSpline[] splines = PathManager.instance.GetComponentsInChildren<BezierSpline>();
        float minDist = 1000;
        int result = -1;
        for(var i=0; i< splines.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, splines[i].GetPoint(0)); //get the distance to the start of the PathManager.instance.Paths[splineIndex]
            if (dist < minDist)
            {
                result = i;
                minDist = dist;
            }
        }

        return result;
    }

    float getProgressForPosition()
    {
        int precision = 1000;
        float minDist = 1000;
        float result = 0;
        for(int i=0;i<precision;i++)
        {
            float p = (i * 1f / (precision - 1));
            Vector3 pos = getPosInPath(p);
            float dist = Vector3.Distance(transform.position, pos);
            if (dist < minDist)
            {
                result = p;
                minDist = dist;
            }
        }

        return result;
    }
}
