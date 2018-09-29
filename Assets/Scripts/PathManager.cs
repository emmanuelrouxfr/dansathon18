using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour {

    static public PathManager instance;

    [Header("Network settings")]
    public string OutputIP;
    public int OutputPort;

    [Header("Paths settings")]
    public List<Vector2> LinkedPaths;

    public List<BezierSpline> Paths;

    public bool LeftPathStarted;
    public bool RightPathStarted;

    void Awake()
    {
        instance = this;
    }

    //public void HandleLeftMeeting(GameObject splineViewer, Vector3 meetingPoint)
    //{
    //    Debug.Log("REACHED LEFT");
    //    LeftPathStarted = false;


    //    //EXAMPLE
    //    splineViewer.GetComponent<MeshRenderer>().enabled = false;
    //}

    //public void HandleRightMeeting(GameObject splineViewer, Vector3 meetingPoint)
    //{
    //    Debug.Log("REACHED RIGHT");
    //    RightPathStarted = false;
        
        
    //    //EXAMPLE
    //    splineViewer.GetComponent<MeshRenderer>().enabled = false;
    //}

    //public void StartLeftPath(GameObject AugmentaPoint)
    //{
    //    if (LeftPathStarted) return;

    //    var selectedLeftPath = Random.Range(0, LeftPaths.Count);
      
    //    var splineViewer = LeftPaths[selectedLeftPath].transform.GetChild(0);
    //    splineViewer.GetComponent<SplineWalker>().Started = true;
    //    splineViewer.GetComponent<SplineWalker>().meetingPointReached += HandleLeftMeeting;
    //    splineViewer.GetComponent<SplineWalker>().LinkedAugmentaPoint = AugmentaPoint.transform;

    //    LeftPathStarted = true;



    //    //EXAMPLE
    //    splineViewer.GetComponent<MeshRenderer>().enabled = true;
    //}

    //public void StartRightPath(GameObject AugmentaPoint)
    //{
    //    if (RightPathStarted) return;

    //    var selectedRightPath = Random.Range(0, RightPaths.Count);
    //    var splineViewer = RightPaths[selectedRightPath].transform.GetChild(0);
    //    splineViewer.GetComponent<SplineWalker>().Started = true;
    //    splineViewer.GetComponent<SplineWalker>().meetingPointReached += HandleRightMeeting;
    //    splineViewer.GetComponent<SplineWalker>().LinkedAugmentaPoint = AugmentaPoint.transform;
    //    RightPathStarted = true;



    //    //EXAMPLE
    //    splineViewer.GetComponent<MeshRenderer>().enabled = true;
    //}

    private void Update()
    {
        SplineWalker.TargetIP = OutputIP;
        SplineWalker.TargetPort = OutputPort;
    }
}
