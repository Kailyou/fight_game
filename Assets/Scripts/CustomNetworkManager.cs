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

	public override void OnServerDisconnect(NetworkConnection conn) {
		StopHost ();
	}

	// called when a new player is added for a client
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		bool isFirst = numPlayers == 0;

		// get start positions
		Transform pos1 = null;
		Transform pos2 = null;
		foreach(Transform pos in startPositions) {
			if(pos.name=="spawn1")
				pos1 = pos;
			else if(pos.name=="spawn2")
				pos2 = pos;
		}

		// determine start position for this player
		Transform startPosition = isFirst ? pos1 : pos2;

		var playerObject = (GameObject)GameObject.Instantiate(playerPrefab, startPosition.position, Quaternion.identity);
		Player player = playerObject.GetComponent<Player> ();
		player.id = numPlayers;

		NetworkServer.AddPlayerForConnection(conn, playerObject, playerControllerId);
	}
}
