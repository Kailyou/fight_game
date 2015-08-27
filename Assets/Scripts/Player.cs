using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	// configuration
	private float speed = 100;
	private float maxVelocity = 100;
	private float jumpPower = 150;

	// refs
	private Rigidbody2D rb2d;
	private Animator animator;

	// set by GroundCheck
	public bool grounded = false;

	// Use this for initialization
	void Start () {
		rb2d = gameObject.GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis ("Horizontal");

		animator.SetBool ("grounded", grounded);
		animator.SetFloat ("speed", Mathf.Abs(h));

		if (h < -0.1) {
			transform.localScale = new Vector3 (-1, 1, 1);
		}
		if (h > 0.1) {
			transform.localScale = new Vector3 (1, 1, 1);
		}

	}

	void FixedUpdate() {
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		rb2d.AddForce (Vector2.right * speed * h);

		if (rb2d.velocity.x > maxVelocity) {
			rb2d.velocity = new Vector2(maxVelocity, rb2d.velocity.y);
		}
		else if (rb2d.velocity.x < -maxVelocity) {
			rb2d.velocity = new Vector2(-maxVelocity, rb2d.velocity.y);
		}
	}
}
