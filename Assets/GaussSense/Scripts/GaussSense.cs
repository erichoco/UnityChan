using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;

public class GaussSense : MonoBehaviour {
	private WebSocket ws;
	private bool wsConnected = false;
	private GData northPoint;
	private GData southPoint;
	private GData bipolarMidpoint;
	private List<int> tagID;


	// Use this for initialization
	void Start () {
		ws = new WebSocket("ws://localhost:5100");

		ws.OnOpen += (object sender, System.EventArgs e) => {
			wsConnected = true;
		};
		ws.OnClose += (object sender, CloseEventArgs e) => {
			wsConnected = false;
		};
		ws.OnMessage += (object sender, MessageEventArgs e) => {
			JSONObject data = new JSONObject(e.Data);
			northPoint = new GData(data["northPoint"]["x"].n, data["northPoint"]["y"].n, data["northPoint"]["intensity"].n, data["northPoint"]["angle"].n, data["northPoint"]["pitch"].n);
			southPoint = new GData(data["southPoint"]["x"].n, data["southPoint"]["y"].n, data["southPoint"]["intensity"].n, data["southPoint"]["angle"].n, data["southPoint"]["pitch"].n);
			bipolarMidpoint = new GData(data["bipolarMidpoint"]["x"].n, data["bipolarMidpoint"]["y"].n, data["bipolarMidpoint"]["intensity"].n, data["bipolarMidpoint"]["angle"].n, data["bipolarMidpoint"]["pitch"].n);
			tagID = new List<int>();
			for (int i = 0; i < data["tagID"].Count; i++) {
				tagID.Add((int)data["tagID"][i].n);
			}
		};
	}

	// Update is called once per frame
	void Update () {
		if (!wsConnected) {
			ws.Connect();
		}
	}

	public GData getNorthPoint() {
		return this.northPoint;
	}

	public GData getSouthPoint() {
		return this.southPoint;
	}

	public GData getBipolarMidpoint() {
		return this.bipolarMidpoint;
	}

	public List<int> getTagID() {
		return this.tagID;
	}
}


public class GData {
	public float x, y, intensity, angle, pitch;

	public GData(float _x, float _y, float _intensity, float _angle, float _pitch){
		this.x = _x;
		this.y = _y;
		this.intensity = _intensity;
		this.angle = _angle;
		this.pitch = _pitch;
	}


	public GData() {
		this.x = 0;
		this.y = 0;
		this.intensity = 0;
		this.angle = 0;
		this.pitch = 0;
	}

	public int getPolarity() {
		if(this.intensity < 0) {
			return 1;
		}
		return 0;
	}

	public float getX() {
		return x;
	}

	public float getY() {
		return y;
	}

	public float getIntensity() {
		return Math.Abs(intensity);
	}

	public float getIntensityInGauss() {
		return getIntensity()*(10f/17f);
	}

	public float getAngle() {
		return angle;
	}

	public float getPitch() {
		return pitch;
	}

}