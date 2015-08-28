using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerAttack : NetworkBehaviour {
	// config
	private float attackCd = 0.3f;

	// status
	[SyncVar] private bool attacking = false;
	private float attacktimer = 0;

	// refs
	public Collider2D attackTrigger;
	private Animator animator;

	[Command]
	void CmdSetData(bool attacking) {
		if (!isServer)
			return;
		
		this.attacking = attacking;
	}
	
	[ClientCallback]
	public void CbSendData() {
		if (isLocalPlayer) {
			CmdSetData (attacking);
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (!isLocalPlayer)
			return;

		// out player collides with sth. hurtful
		if (collider.CompareTag ("DamageTrigger")) {
			// this even works if we are side-by-side with the player script
			gameObject.SendMessageUpwards("TakeDamage", 20);
		}
	}

	void Awake() {
		animator = gameObject.GetComponent<Animator> ();
		attackTrigger.enabled = false;
	}

	void Update () {
		if (isLocalPlayer) {
			if (Input.GetButtonDown ("Attack") && !attacking) {
				attacking = true;
				attacktimer = attackCd;
			}

			if (attacking) {
				if (attacktimer > 0) {
					attacktimer -= Time.deltaTime;
				} else {
					attacking = false;
				}
			}
		}

		// enable collider on all clients so the players can do their collision checks
		attackTrigger.enabled = attacking;

		// publish attack status to animator
		animator.SetBool ("Attacking", attacking);
	}

	void FixedUpdate() {
		CbSendData ();
	}
}
