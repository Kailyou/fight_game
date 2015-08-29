using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PauseMenu : MonoBehaviour {
	public GameObject pauseUI;

	private bool paused = false;
	private CustomNetworkManager networkManager;

	// Use this for initialization
	void Start () {
		pauseUI.SetActive (paused);
		networkManager = GameObject.Find ("Networking").GetComponent<CustomNetworkManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Pause")) {
			paused = !paused;
		}

		pauseUI.SetActive (paused);
	}

	public void onResume() {
		paused = false;
	}

	public void onLeaveGame() {
		networkManager.StopHost ();
	}

	public void onExit() {
		Application.Quit ();
	}
}
