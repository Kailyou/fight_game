using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameoverMenu : MonoBehaviour {
	public GameObject gameoverUI;
	private Text title;
	
	private bool enabled = false;
	private CustomNetworkManager networkManager;
	
	// Use this for initialization
	void Start () {
		gameoverUI.SetActive (enabled);
		networkManager = GameObject.Find ("Networking").GetComponent<CustomNetworkManager> ();
		title = gameoverUI.transform.Find("Panel").Find("Title").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		gameoverUI.SetActive (enabled);
	}
	
	public void onDisconnect() {
		networkManager.StopHost ();
	}
	
	public void onExit() {
		Application.Quit ();
	}
	
	public void onGameOver(bool won) {
		enabled = true;
		title.text = (won?"You won":"You loose");
	}
}
