using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	public GameObject pauseUI;

	private bool paused = false;

	// Use this for initialization
	void Start () {
		pauseUI.SetActive (paused);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Pause")) {
			paused = !paused;
		}

		pauseUI.SetActive (paused);
		Time.timeScale = paused?0:1;
	}

	public void onResume() {
		paused = false;
	}

	public void onMainMenu() {
	}

	public void onExit() {
		Application.Quit ();
	}
}
