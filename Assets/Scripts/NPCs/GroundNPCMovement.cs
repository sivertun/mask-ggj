using UnityEngine;

public class GroundNPCMovement : NPCMovement
{
	[SerializeField] private float leftRange;
	[SerializeField] private float rightRange;
	[SerializeField] private float npcVelocity;
	private Rigidbody2D rb;
	private float startX;
	private float leftTurnX;
	private float rightTurnX;

	private Animator animator;
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		startX = rb.position.x;
		leftTurnX = startX - leftRange;
		rightTurnX = startX + rightRange;
	}
	void FixedUpdate()
	{
		if(!doMove) {
			return;
		}
		animator.SetBool("isWalking", true);
		float currentX = rb.position.x;
		if(
			// To the left and moving left
			(npcVelocity < 0 && currentX <= leftTurnX) ||
			// to the right and moving right 
			(npcVelocity > 0 && currentX >= rightTurnX)
		) {
			npcVelocity = -npcVelocity;

		}
		if(npcVelocity <= 0) {
			FaceLeft();
		} else {
			FaceRight();
		}
		rb.linearVelocityX = npcVelocity;
	}
    void FaceRight()
    {
    	Vector3 scale = transform.localScale;
    	scale.x = Mathf.Abs(scale.x);
    	transform.localScale = scale;
    }

    void FaceLeft()
    {
	Vector3 scale = transform.localScale;
    	scale.x = -Mathf.Abs(scale.x);
    	transform.localScale = scale;
    }
}
