using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	// configuration
	private float maxVelocity = 3;
	private float speed = 150;
	private float jumpPower = 250f;

	// refs
	private Rigidbody2D rb2d;
	private Animator animator;

	// set by GroundCheck
	public bool grounded = false;

	private bool isMirrored = false;
	private bool canDoubleJump = false;

	// Use this for initialization
	void Start () {
		rb2d = gameObject.GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		// publish data to animator
		animator.SetBool ("grounded", grounded);
		animator.SetFloat ("speed", Mathf.Abs(rb2d.velocity.x));

		// walk left image transformation
		if (!isMirrored && h < -0.1) {
			Vector3 tmp = transform.localScale;
			transform.localScale = new Vector3(tmp.x*-1, tmp.y, tmp.z);
			isMirrored = true;
		}
		// walk right image transformation
		if (isMirrored && h > 0.1) {
			Vector3 tmp = transform.localScale;
			transform.localScale = new Vector3(tmp.x*-1, tmp.y, tmp.z);
			isMirrored = false;
		}

		// jump
		if (Input.GetButtonDown("Jump")) {
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

		// calculate camera edges
		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
		Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(
			Camera.main.pixelWidth, Camera.main.pixelHeight));
		// create camera rect
		Rect cameraRect = new Rect(
			bottomLeft.x,
			bottomLeft.y,
			topRight.x - bottomLeft.x,
			topRight.y - bottomLeft.y);
		// clamp player to camera bounds
		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, cameraRect .xMin, cameraRect .xMax),
			Mathf.Clamp(transform.position.y, cameraRect .yMin, cameraRect .yMax),
			transform.position.z);
	}

	void FixedUpdate() {
		float h = Input.GetAxis ("Horizontal");

		// stop walking immediatly when the user releases the button
		Vector3 tmp = rb2d.velocity;
		tmp.x *= 0.75f;
		rb2d.velocity = tmp;

		// walk left/right
		rb2d.AddForce (Vector2.right * speed * h);
		if (rb2d.velocity.x > maxVelocity) {
			rb2d.velocity = new Vector2(maxVelocity, rb2d.velocity.y);
		}
		else if (rb2d.velocity.x < -maxVelocity) {
			rb2d.velocity = new Vector2(-maxVelocity, rb2d.velocity.y);
		}
	}
}
