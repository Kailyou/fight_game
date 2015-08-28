using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CameraClampSync : NetworkBehaviour {
	// sync
	[SyncVar] private Vector3 syncBottomLeft;
	[SyncVar] private Vector3 syncTopRight;
	[SyncVar] private bool hasRect = false;

	[Command]
	void CmdSetRect(Vector3 bottomLeft, Vector3 topRight) {
		if (!hasRect) {
			syncBottomLeft = bottomLeft;
			syncTopRight = topRight;
			hasRect = true;
			return;
		}

		if(bottomLeft.x > syncBottomLeft.x)
			syncBottomLeft.x = bottomLeft.x;
		if(bottomLeft.y > syncBottomLeft.y)
			syncBottomLeft.y = bottomLeft.y;
		if(topRight.x < syncTopRight.x)
			syncTopRight.x = topRight.x;
		if(topRight.y < syncTopRight.y)
			syncTopRight.y = topRight.y;
	}
	
	[ClientCallback]
	void CbSendRect() {
		if (isLocalPlayer) {
			// calculate camera edges
			Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
			Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(
				Camera.main.pixelWidth, Camera.main.pixelHeight));
			CmdSetRect (bottomLeft, topRight);
		}
	}
	
	void Start () {
		CbSendRect ();
	}

	void Update () {
		if(!isLocalPlayer) {
			return;
		}

		// create camera rect
		Rect localRect = new Rect(
			syncBottomLeft.x,
			syncBottomLeft.y,
			syncTopRight.x - syncBottomLeft.x,
			syncTopRight.y - syncBottomLeft.y);

		// clamp player to camera bounds
		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, localRect.xMin, localRect.xMax),
			Mathf.Clamp(transform.position.y, localRect.yMin, localRect.yMax),
			transform.position.z);
	}
}
