using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//
// アニメーション簡易プレビュースクリプト
// 2015/4/25 quad arrow
//

// Require these components when using this script
[RequireComponent(typeof(Animator))]

public class IdleChanger : MonoBehaviour
{

	private Animator anim;						// Animatorへの参照
	private AnimatorStateInfo currentState;		// 現在のステート状態を保存する参照
	private AnimatorStateInfo previousState;	// ひとつ前のステート状態を保存する参照

	private GaussSense gs;
	private List<GData> northPoints;
	private List<GData> southPoints;
	private List<GData> midpoints;

	private int actionBuffer = 0;

	private bool doneJab;
	private bool doneHikick;
	private bool doneRising;
	private bool doneSpinkick;
	private bool doneSamK;

	// Use this for initialization
	void Start ()
	{
		// 各参照の初期化
		anim = GetComponent<Animator> ();
		currentState = anim.GetCurrentAnimatorStateInfo (0);
		previousState = currentState;

		gs = GameObject.Find("GaussSense").GetComponent<GaussSense>();
		midpoints = new List<GData>(10); // track the last 10 midpoints to check different movements
		northPoints = new List<GData>(10);
		southPoints = new List<GData>(10);
	}

	void Awake()
	{
        Application.targetFrameRate = 300;
    }

	void FixedUpdate()
	{
		// Debug.Log(Time.deltaTime);
		GData midpoint = gs.getBipolarMidpoint();
		if (midpoints.Count >= 10)
		{
			midpoints.RemoveAt(0);
		}
		midpoints.Add(midpoint);
		GData northPoint = gs.getNorthPoint();
		if (northPoints.Count >= 10)
		{
			northPoints.RemoveAt(0);
		}
		northPoints.Add(northPoint);
		GData southPoint = gs.getSouthPoint();
		if (southPoints.Count >= 10)
		{
			southPoints.RemoveAt(0);
		}
		southPoints.Add(southPoint);
		if (actionBuffer == 0)
		{
			checkAction();
		}
		else
		{
			actionBuffer--;
		}
	}

	void OnGUI()
	{
		return;
		GUI.Box(new Rect(Screen.width - 200 , 45 ,120 , 350), "");
		if(GUI.Button(new Rect(Screen.width - 190 , 60 ,100, 20), "Jab"))
		{
			Debug.Log("Jab");
			anim.SetBool ("Jab", true);
		}
		if(GUI.Button(new Rect(Screen.width - 190 , 90 ,100, 20), "Hikick"))
			anim.SetBool ("Hikick", true);
		if(GUI.Button(new Rect(Screen.width - 190 , 120 ,100, 20), "Spinkick"))
			anim.SetBool ("Spinkick", true);
		if(GUI.Button(new Rect(Screen.width - 190 , 150 ,100, 20), "Rising_P"))
			anim.SetBool ("Rising_P", true);
		if(GUI.Button(new Rect(Screen.width - 190 , 180 ,100, 20), "Headspring"))
			anim.SetBool ("Headspring", true);
		if(GUI.Button(new Rect(Screen.width - 190 , 210 ,100, 20), "Land"))
			anim.SetBool ("Land", true);
		if(GUI.Button(new Rect(Screen.width - 190 , 240 ,100, 20), "ScrewKick"))
			anim.SetBool ("ScrewK", true);
		if(GUI.Button(new Rect(Screen.width - 190 , 270 ,100, 20), "DamageDown"))
			anim.SetBool ("DamageDown", true);
		if(GUI.Button(new Rect(Screen.width - 190 , 300 ,100, 20), "SamK"))
			anim.SetBool ("SAMK", true);
		if(GUI.Button(new Rect(Screen.width - 190 , 330 ,100, 20), "Run"))
			anim.SetBool ("Run", true);
		if(GUI.Button(new Rect(Screen.width - 190 , 360 ,100, 20), "Run_end"))
			anim.SetBool ("Run", false);
	}

	void checkAction()
	{
		if (midpoints.Count < 10) return;

		GData p = midpoints[midpoints.Count-1];
		GData n = northPoints[northPoints.Count-1];
		GData s = southPoints[southPoints.Count-1];
		// Debug.Log(p.getIntensityInGauss() + " " + p.getIntensity());
		if (p != null && p.getIntensity() > 0)
		{
			bool actionExist = false;

			// Debug.Log(n.getIntensity() + " " + s.getIntensity());
			if (n.getIntensity() > 30 && s.getIntensity() > 30)
			{
				// Spinkick
				if (!actionExist &&
					p.getAngle()*Mathf.Rad2Deg - midpoints[midpoints.Count-2].getAngle()*Mathf.Rad2Deg > 5 &&
					p.getAngle()*Mathf.Rad2Deg - midpoints[midpoints.Count-2].getAngle()*Mathf.Rad2Deg < 180)
				{
					bool spinningClock = true;
					for (int i = midpoints.Count-1; i > midpoints.Count-5; i--)
					{
						float angleA = midpoints[i].getAngle()*Mathf.Rad2Deg;
						float angleB = midpoints[i-1].getAngle()*Mathf.Rad2Deg;
						if (angleA - angleB < 5 || angleA - angleB > 180)
						{
							spinningClock = false;
							break;
						}
					}
					if (spinningClock)
					{
						actionExist = true;
						actionBuffer = 20;
						anim.SetBool("Spinkick", true);
						Debug.Log("Spinkick");
					}
					// string debugMsg = "";
					// for (int i = midpoints.Count-1; i >= 0; i--)
					// {
					// 	debugMsg += midpoints[i].getAngle()*Mathf.Rad2Deg + " ";
					// }
					// Debug.Log(debugMsg);
				}

				// SamK
				if (!actionExist && //p.getIntensity() - midpoints[midpoints.Count-2].getIntensity() < -5 &&
					p.getAngle()*Mathf.Rad2Deg - midpoints[midpoints.Count-2].getAngle()*Mathf.Rad2Deg < -5 &&
					p.getAngle()*Mathf.Rad2Deg - midpoints[midpoints.Count-2].getAngle()*Mathf.Rad2Deg > -180)
				{
					bool jumpingSpinning = true;
					for (int i = midpoints.Count-1; i > midpoints.Count-5; i--)
					{
						float angleA = midpoints[i].getAngle()*Mathf.Rad2Deg;
						float angleB = midpoints[i-1].getAngle()*Mathf.Rad2Deg;
						if ((angleA - angleB > -5 || angleA - angleB < -180))// ||
							//(midpoints[i].getIntensity() > midpoints[i-1].getIntensity() - 5))
						{
							jumpingSpinning = false;
							break;
						}
					}
					if (jumpingSpinning)
					{
						actionExist = true;
						actionBuffer = 20;
						anim.SetBool("SAMK", true);
						Debug.Log("SamK");
					}
					// string debugMsg = "";
					// for (int i = midpoints.Count-1; i >= 0; i--)
					// {
					// 	debugMsg += midpoints[i].getAngle()*Mathf.Rad2Deg + " ";
					// }
					// Debug.Log(debugMsg);
				}
			}

			// Jab
			if (!actionExist && p.getX() > 0.7)
			{
				bool movingRight = true;
				for (int i = midpoints.Count-1; i > midpoints.Count-3; i--)
				{
					// Debug.Log(midpoints[i].getX() - midpoints[i-1].getX());
					if (midpoints[i].getX() < midpoints[i-1].getX() + 0.02)
					{
						movingRight = false;
						break;
					}
				}
				if (movingRight)
				{
					actionBuffer = 20;
					actionExist = true;
					anim.SetBool("Jab", true);
					Debug.Log("Jab");
				}
				// Debug.Log("");

				// string debugMsg = "";
				// for (int i = midpoints.Count-1; i >= 0; i--)
				// {
				// 	debugMsg += midpoints[i].getX() + " ";
				// }
				// Debug.Log(debugMsg);
			}

			// Hikick
			if (!actionExist && p.getY() < 0.3)
			{
				bool movingUp = true;
				for (int i = midpoints.Count-1; i > midpoints.Count-3; i--)
				{
					if (midpoints[i].getY() > midpoints[i-1].getY() - 0.03)
					{
						movingUp = false;
						break;
					}
				}
				if (movingUp)
				{
					actionExist = true;
					actionBuffer = 20;
					anim.SetBool("Hikick", true);
					Debug.Log("Hikick");
				}
				// string debugMsg = "";
				// for (int i = midpoints.Count-1; i >= 0; i--)
				// {
				// 	debugMsg += midpoints[i].getY() + " ";
				// }
				// Debug.Log(debugMsg);
			}

			// Rising P
			if (!actionExist && p.getIntensity() < 15)
			{
				bool jumping = true;
				for (int i = midpoints.Count-1; i > midpoints.Count-3; i--)
				{
					if (midpoints[i].getIntensity() - midpoints[i-1].getIntensity() > -10 ||
						Mathf.Abs(midpoints[i].getX() - midpoints[i-1].getX()) > 0.2f ||
						Mathf.Abs(midpoints[i].getY() - midpoints[i-1].getY()) > 0.2f)
					{
						// string debugMsg = "";
						// for (int j = midpoints.Count-1; j >= 0; j--)
						// {
						// 	debugMsg += midpoints[j].getX() + " ";
						// }
						// Debug.Log(debugMsg);
						// Debug.Log(Mathf.Abs(midpoints[i].getX() - midpoints[i-1].getX()));
						// Debug.Log(Mathf.Abs(midpoints[i].getY() - midpoints[i-1].getY()));
						jumping = false;
						break;
					}
				}
				if (jumping)
				{
					actionExist = true;
					actionBuffer = 20;
					anim.SetBool("Rising_P", true);
					Debug.Log("Rising P");
				}
				// string debugMsg = "";
				// for (int j = midpoints.Count-1; j >= 0; j--)
				// {
				// 	debugMsg += midpoints[j].getX() + " ";
				// }
				// Debug.Log(debugMsg);
				// string debugMsg = "";
				// for (int i = midpoints.Count-1; i >= 0; i--)
				// {
				// 	debugMsg += midpoints[i].getIntensity() + " ";
				// }
				// Debug.Log(debugMsg);
			}
		}
	}

}
