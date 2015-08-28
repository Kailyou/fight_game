using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

	public override void OnServerConnect(NetworkConnection conn) {
		if (numPlayers >= 2) {
			conn.Disconnect();
			return;
		}
	}
}
