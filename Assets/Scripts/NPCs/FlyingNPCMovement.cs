using UnityEngine;

public class FlyingNPCMovement : NPCMovement
{
	[SerializeField] private int range;
	[SerializeField] private Vector2 npcVelocity;
	private Vector2 startPosition;
	private Rigidbody2D rb;
	private bool neverMoveAgain = false;
	private float ogGravityScale;

	public void Start() {
		rb = GetComponent<Rigidbody2D>();
		startPosition = rb.position;
		ogGravityScale = rb.gravityScale;
		rb.gravityScale = 0;
	}

	public void FixedUpdate() {
		Vector2 normalVector = rb.position - startPosition;
		if(
			// Pointing the same direction
			// and
			// out of range
			Vector2.Dot(normalVector, npcVelocity) >= 0 &&
			Vector2.Distance(startPosition, rb.position) >= range
		) {
			npcVelocity = -npcVelocity;
		}
		if(doMove && !neverMoveAgain) {
			rb.gravityScale = 0;
			rb.linearVelocity = npcVelocity;
		} else {
			rb.gravityScale = ogGravityScale;
			neverMoveAgain = true;
		}
	}
}
