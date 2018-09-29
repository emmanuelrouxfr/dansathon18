using UnityEngine;
using UnityOSC;

public class SplineWalker : MonoBehaviour {

    public static string TargetIP;
    public static int TargetPort;

    public delegate void OnMeetingPointReached(GameObject splineViewer, Vector3 position);
    public event OnMeetingPointReached meetingPointReached;

    [Header("Augmenta")]
    public Transform LinkedAugmentaPoint;
    public float DistanceToAugmentaPoint;
	public BezierSpline spline;

    [Header("Walker settings")]

    public bool Started;
    public bool lookForward;

    [Range(0.0f, 1.0f)]
	public float progress;
	private bool goingForward = true;

    private void Start()
    {
        Init();
    }

    public void Init() {
        progress = 0.0f;
        Started = false;
        Vector3 position = spline.GetPoint(progress);
        transform.position = position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, DistanceToAugmentaPoint);
    }

    private void Update () {


        if (LinkedAugmentaPoint == null || !Started)
            return;

        AugmentaPersonBehaviour apb = LinkedAugmentaPoint.GetComponent<AugmentaPersonBehaviour>();
        if (apb != null)
        {
            OSCMessage msg = new OSCMessage("/splineViewer/" + apb.pid, progress);
            OSCMaster.sendMessage(msg, TargetIP, TargetPort);
        }
        else
        {
            Debug.LogWarning("AugmentaPersonBehaviour should not be null");
        }
       
        if ((LinkedAugmentaPoint.position - transform.position).magnitude < DistanceToAugmentaPoint)
            progress += DistanceToAugmentaPoint - (LinkedAugmentaPoint.position - transform.position).magnitude;

        progress = Mathf.Clamp(progress, 0.0f, 1.0f);

		Vector3 position = spline.GetPoint(progress);
		transform.position = position;
		if (lookForward) {
			transform.LookAt(position + spline.GetDirection(progress));
		}

		if (progress >= 1f) {

            if (meetingPointReached != null)
                meetingPointReached(this.gameObject, transform.position);

            Init();
		}
	}
}