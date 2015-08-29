using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerAttack : NetworkBehaviour {
	// config
	private float attackCd = 0.3f;
	private float attackedCd = 0.3f;

	// status
	private bool attacking = false;
	private bool attacked = false;
	[SyncVar] private bool syncAttacking = false;
	[SyncVar] private bool syncAttacked = false;
	private float attacktimer = 0;
	private float attackedtimer = 0;

	// refs
	public Collider2D attackTriggerFront;
	public Collider2D attackTriggerBack;
	private Animator animator;

	[Command]
	void CmdSetData(bool attacking, bool attacked) {
		if (!isServer)
			return;
		
		syncAttacking = attacking;
		syncAttacked = attacked;
	}
	
	[ClientCallback]
	public void CbSendData() {
		if (isLocalPlayer) {
			CmdSetData (attacking, attacked);
		}
	}

	public void applyRemoteData() {
		if (!isLocalPlayer) {
			attacking = syncAttacking;
			attacked = syncAttacked;
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (!isLocalPlayer)
			return;

		// out player collides with sth. hurtful
		if (collider.CompareTag ("DamageTrigger")) {
			// this even works if we are side-by-side with the player script
			gameObject.SendMessageUpwards("TakeDamage", 20);
			attacked = true;
			attackedtimer = attackedCd;
		}
	}

	void Awake() {
		animator = gameObject.GetComponent<Animator> ();
		attackTriggerFront.enabled = false;
		attackTriggerBack.enabled = false;
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

			if(attacked) {
				if (attackedtimer > 0) {
					attackedtimer -= Time.deltaTime;
				} else {
					attacked = false;
				}
			}
		}

		// enable collider on all clients so the players can do their collision checks
		attackTriggerFront.enabled = attacking;
		attackTriggerBack.enabled = attacking;

		// publish attack status to animator
		animator.SetBool ("Attacking", attacking);
		animator.SetBool ("Attacked", attacked);
	}

	void FixedUpdate() {
		CbSendData ();
		applyRemoteData ();
	}
}
