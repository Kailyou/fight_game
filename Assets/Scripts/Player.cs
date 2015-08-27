using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	// configuration
	private float maxVelocity = 3;
	private float speed = 50;
	private float jumpPower = 5;

	// refs
	private Rigidbody2D rb2d;
	private Animator animator;

	// set by GroundCheck
	public bool grounded = false;

	private bool isMirrored = false;

	// Use this for initialization
	void Start () {
		rb2d = gameObject.GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		animator.SetBool ("grounded", grounded);
		animator.SetFloat ("speed", Mathf.Abs(h));

		if (!isMirrored && h < -0.1) {
			Vector3 tmp = transform.localScale;
			transform.localScale = new Vector3(tmp.x*-1, tmp.y, tmp.z);
			isMirrored = true;
		}
		if (isMirrored && h > 0.1) {
			Vector3 tmp = transform.localScale;
			transform.localScale = new Vector3(tmp.x*-1, tmp.y, tmp.z);
			isMirrored = false;
		}

		if (grounded && v>0.1) {
			rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower);
		}

	}

	void FixedUpdate() {
		float h = Input.GetAxis ("Horizontal");

		rb2d.AddForce (Vector2.right * speed * h);

		if (rb2d.velocity.x > maxVelocity) {
			rb2d.velocity = new Vector2(maxVelocity, rb2d.velocity.y);
		}
		else if (rb2d.velocity.x < -maxVelocity) {
			rb2d.velocity = new Vector2(-maxVelocity, rb2d.velocity.y);
		}
	}
}
