using UnityEngine;
using System.Collections;

public class Healthbar : MonoBehaviour {
	private Transform fill1;
	private Transform fill2;
	private SpriteRenderer fill1Renderer;
	private SpriteRenderer fill2Renderer;
	
	private Vector3 fill1InitialScale;
	private Vector3 fill2InitialScale;

	public int maxValue = 100;

	// Use this for initialization
	void Start () {
		fill1 = transform.Find ("health_bar_fill1");
		fill2 = transform.Find ("health_bar_fill2");
		fill1Renderer = fill1.GetComponent<SpriteRenderer> ();
		fill2Renderer = fill2.GetComponent<SpriteRenderer> ();

		fill1InitialScale = fill1.localScale;
		fill2InitialScale = fill2.localScale;
	}

	public void setValue(int val) {
		float totalWidthMax = fill1InitialScale.x + fill2InitialScale.x;
		float totalWidthVal = totalWidthMax / maxValue * val;
		float totalWidthVal1 = 0;
		float totalWidthVal2 = 0;

		if (totalWidthVal > fill1InitialScale.x) {
			totalWidthVal1 = fill1InitialScale.x;
			totalWidthVal2 = totalWidthVal - fill2InitialScale.x;
		} else {
			totalWidthVal1 = totalWidthVal;
			totalWidthVal2 = 0;
		}

		fill1.localScale = new Vector3 (totalWidthVal1, fill1.localScale.y, fill1.localScale.z);
		fill2.localScale = new Vector3 (totalWidthVal2, fill2.localScale.y, fill2.localScale.z);

		if (val < maxValue / 8) {
			fill2Renderer.color = fill1Renderer.color = new Color32 (255, 75, 75, 255);
		} else if (val < maxValue / 2) {
			fill2Renderer.color = fill1Renderer.color = new Color32 (255, 255, 77, 255);
		} else {
			fill2Renderer.color = fill1Renderer.color = new Color32 (75, 255, 99, 255);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
