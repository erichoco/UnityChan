using UnityEngine;
using System.Collections;

public class GetBasicPoints : MonoBehaviour {

	private GaussSense gs;
	private GameObject redPoint;
	private GameObject bluePoint;
	private GameObject midPoint;

	// Use this for initialization
	void Start () {
		gs = GameObject.Find("GaussSense").GetComponent<GaussSense>();
		redPoint = GameObject.Find ("NorthPoint");
		bluePoint = GameObject.Find ("SouthPoint");
		midPoint = GameObject.Find ("BipolarMidpoint");
	}

	// Update is called once per frame
	void Update () {
		GData northPoint = gs.getNorthPoint();
		redPoint.transform.position = new Vector3 (-(northPoint.getX ()*6 - 3), northPoint.getY ()*6 - 3, 0.0f);

		GData southPoint = gs.getSouthPoint();
		bluePoint.transform.position = new Vector3 (-(southPoint.getX ()*6 - 3), southPoint.getY ()*6 - 3, 0.0f);

		GData bipolarMidpoint = gs.getBipolarMidpoint();
		midPoint.transform.position = new Vector3 (-(bipolarMidpoint.getX ()*6 - 3), bipolarMidpoint.getY ()*6 - 3, 0.0f);
		midPoint.transform.rotation =  Quaternion.Euler(0.0f, 0.0f, -bipolarMidpoint.getAngle ()*Mathf.Rad2Deg);

	}
}
