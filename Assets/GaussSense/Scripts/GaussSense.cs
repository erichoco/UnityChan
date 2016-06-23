using UnityEngine;
using System;
using System.Collections;
using WebSocketSharp;
using WebSocketSharp.Net;
using SocketIO;

public class GaussSense : MonoBehaviour {
	private SocketIOComponent socket;
	private GData northPoint;
	private GData southPoint;
	private GData bipolarMidpoint;

	// Use this for initialization
	void Start () {
		GameObject io = GameObject.Find("SocketIO");
		socket = io.GetComponent<SocketIOComponent>();

		socket.On("northPoint", (SocketIOEvent e) => {
			northPoint = new GData(e.data["x"].n, e.data["y"].n, e.data["intensity"].n, e.data["angle"].n, e.data["pitch"].n);
		});

		socket.On("southPoint", (SocketIOEvent e) => {
			southPoint = new GData(e.data["x"].n, e.data["y"].n, e.data["intensity"].n, e.data["angle"].n, e.data["pitch"].n);
		});

		socket.On("bipolarMidpoint", (SocketIOEvent e) => {
			bipolarMidpoint = new GData(e.data["x"].n, e.data["y"].n, e.data["intensity"].n, e.data["angle"].n, e.data["pitch"].n);
		});
	}
	
	// Update is called once per frame
	void Update () {
		
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