using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour {

	private Image image;
	private UnityChan chan;
	private int tutorialState = 0;

	private IEnumerator smoothBlink;

	void Start () {
		image = GetComponent<Image>();
		chan = GameObject.Find("Package04_animChanger").GetComponent<UnityChan>();
		smoothBlink = SmoothBlink();
		StartCoroutine(smoothBlink);
	}

	void Update () {
		switch (tutorialState) {
			case 0:
				if (chan.actionState == 1) {
					ChangeImage("Sprites/forward");
					tutorialState++;
				}
				break;
			case 1:
				if (chan.actionState == 2) {
					ChangeImage("Sprites/up");
					tutorialState++;
				}
				break;
			case 2:
				if (chan.actionState == 3) {
					ChangeImage("Sprites/cw");
					tutorialState++;
				}
				break;
			case 3:
				if (chan.actionState == 4) {
					ChangeImage("Sprites/ccw");
					tutorialState++;
				}
				break;
			case 4:
				if (chan.actionState == 5) {
					StopCoroutine(smoothBlink);
					image.enabled = false;
					tutorialState++;
				}
				break;
			default:
				break;
		}
	}

	IEnumerator SmoothBlink() {
		CanvasRenderer renderer = GetComponent<CanvasRenderer>();
		while (true) {
			for (float f = 1f; f > 0; f -= 0.1f) {
				renderer.SetAlpha(f);
				yield return new WaitForSeconds(0.04f);
			}
			// yield return new WaitForSeconds(0.4f);
			for (float f = 0; f <= 1.1f; f += 0.1f) {
				renderer.SetAlpha(f);
				yield return new WaitForSeconds(0.04f);
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	void ChangeImage(string title){
		Sprite newSprite = Resources.Load<Sprite>(title);
		if (newSprite) {
			image.sprite = newSprite;
		} else {
			Debug.Log("Sprite not found.");
		}
	}

}
