using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
	// configuration
	private float maxVelocity = 3;
	private float speed = 150;
	private float jumpPower = 250f;
	private int maxHealth = 100;

	// refs
	private Rigidbody2D rb2d;
	private Animator animator;

	// status vars
	private bool isMirrored = false;
	private bool canDoubleJump = false;
	[SyncVar] private int syncHealth;
	[SyncVar] private bool syncCrouching;
	private Healthbar healthbar;

	// externally set
	public bool grounded = false;
	[SyncVar] public int id = -1;

	[Command]
	void CmdTakeDamage(int amount) {
		if (!isServer)
			return;

		syncHealth -= amount;
	}

	[ClientCallback]
	public void TakeDamage(int amount) {
		if (isLocalPlayer) {
			CmdTakeDamage (amount);
		}
	}

	[Command]
	void CmdSetData(bool crouching) {
		if (!isServer)
			return;
		
		syncCrouching = crouching;
	}

	[ClientCallback]
	public void CbSendData() {
		if (isLocalPlayer) {
			CmdSetData (Input.GetAxis ("Vertical")<-0.1);
		}
	}
	
	void Start () {
		rb2d = gameObject.GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<Animator>();
		healthbar = GameObject.FindGameObjectWithTag (id==0?"Healthbar1":"Healthbar2").GetComponent<Healthbar> ();

		syncHealth = maxHealth;
		healthbar.maxValue = maxHealth;
		healthbar.setValue (syncHealth);

		if (id == 1) {
			Vector3 tmp = transform.localScale;
			transform.localScale = new Vector3(tmp.x*-1, tmp.y, tmp.z);
			isMirrored = true;
		}
	}
	
	void Update () {
		// publish data to animator
		animator.SetBool ("grounded", grounded);
		animator.SetFloat ("speed", Mathf.Abs(rb2d.velocity.x));
		animator.SetBool ("Crouching", syncCrouching);

		// publish data to healthbar
		healthbar.setValue (syncHealth);

		if (!isLocalPlayer) {
			return;
		}

		if (syncHealth <= 0) {
			Die ();
		}

		// walk left image transformation
		if (!isMirrored && rb2d.velocity.x < -0.1) {
			Vector3 tmp = transform.localScale;
			transform.localScale = new Vector3(tmp.x*-1, tmp.y, tmp.z);
			isMirrored = true;
		}
		// walk right image transformation
		if (isMirrored && rb2d.velocity.x > 0.1) {
			Vector3 tmp = transform.localScale;
			transform.localScale = new Vector3(tmp.x*-1, tmp.y, tmp.z);
			isMirrored = false;
		}

		// jump
		if (!syncCrouching && Input.GetButtonDown("Jump")) {
			if(grounded) {
				rb2d.AddForce(Vector2.up*jumpPower);
				canDoubleJump = true;
			}
			else if(canDoubleJump) {
				rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
				rb2d.AddForce(Vector2.up*jumpPower);
				canDoubleJump = false;
			}
		}
	}

	void FixedUpdate() {
		if (!isLocalPlayer) {
			return;
		}

		if (!syncCrouching) {
			float h = Input.GetAxis ("Horizontal");

			// stop walking immediatly when the user releases the button
			Vector3 tmp = rb2d.velocity;
			tmp.x *= 0.75f;
			rb2d.velocity = tmp;

			// walk left/right
			rb2d.AddForce (Vector2.right * speed * h);
			if (rb2d.velocity.x > maxVelocity) {
				rb2d.velocity = new Vector2 (maxVelocity, rb2d.velocity.y);
			} else if (rb2d.velocity.x < -maxVelocity) {
				rb2d.velocity = new Vector2 (-maxVelocity, rb2d.velocity.y);
			}
		}

		CbSendData ();
	}

	private void Die() {
	}
}
