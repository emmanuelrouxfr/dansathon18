using UnityEngine;
using System;
using System.Collections.Generic;

public class BezierSpline : MonoBehaviour {

    public int MeetingPointIndex;

	[SerializeField]
	public List<Vector3> points;
    
	[SerializeField]
	private BezierControlPointMode[] modes;

	[SerializeField]
	private bool loop;

	public bool Loop {
		get {
			return loop;
		}
		set {
			loop = value;
			if (value == true) {
				modes[modes.Length - 1] = modes[0];
				SetControlPoint(0, points[0]);
			}
		}
	}

	public int ControlPointCount {
		get {
			return points.Count;
		}
	}

	public Vector3 GetControlPoint (int index) {
		return points[index];
	}

	public void SetControlPoint (int index, Vector3 point) {
		if (index % 3 == 0) {
			Vector3 delta = point - points[index];
			if (loop) {
				if (index == 0) {
					points[1] += delta;
					points[points.Count - 2] += delta;
					points[points.Count - 1] = point;
				}
				else if (index == points.Count - 1) {
					points[0] = point;
					points[1] += delta;
					points[index - 1] += delta;
				}
				else {
					points[index - 1] += delta;
					points[index + 1] += delta;
				}
			}
			else {
				if (index > 0) {
					points[index - 1] += delta;
				}
				if (index + 1 < points.Count) {
					points[index + 1] += delta;
				}
			}
		}
		points[index] = point;
		EnforceMode(index);
	}

	public BezierControlPointMode GetControlPointMode (int index) {
		return modes[(index + 1) / 3];
	}

	public void SetControlPointMode (int index, BezierControlPointMode mode) {
		int modeIndex = (index + 1) / 3;
		modes[modeIndex] = mode;
		if (loop) {
			if (modeIndex == 0) {
				modes[modes.Length - 1] = mode;
			}
			else if (modeIndex == modes.Length - 1) {
				modes[0] = mode;
			}
		}
		EnforceMode(index);
	}

	private void EnforceMode (int index) {
		int modeIndex = (index + 1) / 3;
		BezierControlPointMode mode = modes[modeIndex];
		if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1)) {
			return;
		}

		int middleIndex = modeIndex * 3;
		int fixedIndex, enforcedIndex;
		if (index <= middleIndex) {
			fixedIndex = middleIndex - 1;
			if (fixedIndex < 0) {
				fixedIndex = points.Count - 2;
			}
			enforcedIndex = middleIndex + 1;
			if (enforcedIndex >= points.Count) {
				enforcedIndex = 1;
			}
		}
		else {
			fixedIndex = middleIndex + 1;
			if (fixedIndex >= points.Count) {
				fixedIndex = 1;
			}
			enforcedIndex = middleIndex - 1;
			if (enforcedIndex < 0) {
				enforcedIndex = points.Count - 2;
			}
		}

		Vector3 middle = points[middleIndex];
		Vector3 enforcedTangent = middle - points[fixedIndex];
		if (mode == BezierControlPointMode.Aligned) {
			enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
		}
		points[enforcedIndex] = middle + enforcedTangent;
	}

	public int CurveCount {
		get {
			return (points.Count - 1) / 3;
		}
	}

	public Vector3 GetPoint (float t) {
		int i;
		if (t >= 1f) {
			t = 1f;
			i = points.Count - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
	}
	
	public Vector3 GetVelocity (float t) {
		int i;
		if (t >= 1f) {
			t = 1f;
			i = points.Count - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return transform.TransformPoint(Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
	}
	
	public Vector3 GetDirection (float t) {
		return GetVelocity(t).normalized;
	}

    public void RemovePoint(int selectedIndex)
    {
        if (selectedIndex <= 0 || selectedIndex >= points.Count)
        {
            return;
        }

        //if (selectedIndex % 3 != 2)
        //    return;

        //TOM IMPROVE TO PREVENT TANGEANT DELETION

        points.RemoveAt(selectedIndex - 1);
        points.RemoveAt(selectedIndex);
        points.RemoveAt(selectedIndex + 1);

    }

	public void AddCurve () {
		Vector3 point = points[points.Count - 1];

        //      var newLeftTangent = new GameObject("[+] Tangent");
        //      var newPoint = new GameObject("Point");
        //      var newRightTangent = new GameObject("[+] Tangent");

        //      newLeftTangent.transform.SetParent(this.transform);
        //      newPoint.transform.SetParent(this.transform);
        //      newRightTangent.transform.SetParent(this.transform);

        //      points.Add(newLeftTangent.transform);
        //      points.Add(newPoint.transform);
        //      points.Add(newRightTangent.transform);

        //      point.x += 1f;
        //points[points.Count - 3] = point;
        //point.x += 1f;
        //points[points.Count - 2] = point;
        //point.x += 1f;
        //points[points.Count - 1] = point;
        points.Add(new Vector3(point.x + 1.0f, 0.0f, 0.0f));
        points.Add(new Vector3(point.x + 2.0f, 0.0f, 0.0f));
        points.Add(new Vector3(point.x + 3.0f, 0.0f, 0.0f));


        Array.Resize(ref modes, modes.Length + 1);
		modes[modes.Length - 1] = modes[modes.Length - 2];
		EnforceMode(points.Count - 4);

		if (loop) {
			points[points.Count - 1] = points[0];
			modes[modes.Length - 1] = modes[0];
			EnforceMode(0);
		}
	}
	
	public void Reset () {
        points = new List<Vector3>();
        points.Add(new Vector3(1.0f, 0.0f, 0.0f));
        points.Add(new Vector3(2.0f, 0.0f, 0.0f));
        points.Add(new Vector3(3.0f, 0.0f, 0.0f));
        points.Add(new Vector3(4.0f, 0.0f, 0.0f));
        //var newLeftTangent = new GameObject("[+] Tangent");
        //var newPoint = new GameObject("Point");
        //var newRightTangent = new GameObject("[+] Tangent");

        //newLeftTangent.transform.SetParent(this.transform);
        //newPoint.transform.SetParent(this.transform);
        //newRightTangent.transform.SetParent(this.transform);

        //points.Add(newLeftTangent.transform);
        //points.Add(newPoint.transform);
        //points.Add(newRightTangent.transform);

        modes = new BezierControlPointMode[] {
			BezierControlPointMode.Free,
			BezierControlPointMode.Free
		};
	}
}