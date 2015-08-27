using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour {

	private Player player;

	void Start() {
		player = gameObject.GetComponentInParent<Player> ();

	}

	void OnTriggerEnter2d(Collider2D collider) {
		player.grounded = true;
	}

	void OnTriggerExit2d(Collider2D collider) {
		player.grounded = false;
	}
}
