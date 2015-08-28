using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

	/*public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		GameObject player = (GameObject)Instantiate(playerPrefab, CustomGetStartPosition(), Quaternion.identity);
		//player.GetComponent<Player>().color = Color.Red;
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

	public Transform CustomGetStartPosition() {
		Debug.Log ("lol", gameObject);

		NetworkStartPosition[] positions = gameObject.GetComponents<NetworkStartPosition> ();
		for(int i=0; i<positions.Length; i++) {
			NetworkStartPosition pos = positions[i];
			Debug.Log(pos.name);
			if(pos.name=="spawn_local")
				return pos.;
		}

		return null;
	}*/
}
