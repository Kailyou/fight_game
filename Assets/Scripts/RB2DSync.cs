using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class RB2DSync : NetworkBehaviour {
	// refs
	private Rigidbody2D rb2d;

	// sync
	[SyncVar] private Vector3 syncPos;
	[SyncVar] private Vector3 syncScale;
	[SyncVar] private Vector2 syncVelocity;
	private float lerpRate = 15;
	private bool initialSync = true;
	
	[Command]
	void CmdSetData(Vector3 pos, Vector3 scale, Vector2 velocity) {
		syncPos = pos;
		syncScale = scale;
		syncVelocity = velocity;
	}
	
	[ClientCallback]
	void CbSendData() {
		if (isLocalPlayer) {
			CmdSetData (transform.position, transform.localScale, rb2d.velocity);
		}
	}
	
	void LerpData() {
		if (!isLocalPlayer) {
			if(initialSync) {
				transform.position = syncPos;
				initialSync = false;
			}
			else {
				transform.position = Vector3.Lerp(transform.position, syncPos, Time.deltaTime*lerpRate);
			}

			transform.localScale = syncScale;
			rb2d.velocity = syncVelocity;
		}
	}

	void Start () {
		rb2d = gameObject.GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate () {
		CbSendData();
		LerpData();
	}
}
