using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour {

	private Player player;

	void Start() {
		player = gameObject.GetComponentInParent<Player> ();

	}

	void OnTriggerEnter2D(Collider2D collider) {
		player.grounded = true;
		player.TakeDamage (5);
	}

	void OnTriggerStay2D(Collider2D collider) {
		player.grounded = true;
	}

	void OnTriggerExit2D(Collider2D collider) {
		player.grounded = false;
	}
}
