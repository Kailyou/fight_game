using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	public GameObject mainUI;
	public GameObject joinUI;
	public GameObject hostInputFieldObject;

	private NetworkManager networkManager;
	private InputField hostInputField;
	
	// Use this for initialization
	void Start () {
		mainUI.SetActive (true);
		joinUI.SetActive (false);
		networkManager = GameObject.Find ("Networking").GetComponent<NetworkManager> ();
		hostInputField = hostInputFieldObject.GetComponent<InputField> ();
	}
	
	public void showJoinUI() {
		hostInputField.text = networkManager.networkAddress;
		mainUI.SetActive (false);
		joinUI.SetActive (true);
	}

	public void showMainUI() {
		mainUI.SetActive (true);
		joinUI.SetActive (false);
		networkManager.StopClient ();
	}

	public void onHostGame() {
		networkManager.StartHost ();
	}
	
	public void onExit() {
		Application.Quit ();
	}

	public void onJoin() {
		networkManager.networkAddress = hostInputField.text;
		networkManager.StartClient ();
	}
}
