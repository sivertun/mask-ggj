using UnityEngine;

public class NPCMovement : MonoBehaviour
{
	[SerializeField] private float leftRange;
	[SerializeField] private float rightRange;
	[SerializeField] private float npcVelocity;
	private Rigidbody2D rb;
	private float startX;
	private float leftTurnX;
	private float rightTurnX;
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		startX = rb.position.x;
		leftTurnX = startX - leftRange;
		rightTurnX = startX + rightRange;
	}
	void FixedUpdate()
	{
		float currentX = rb.position.x;
		if(
			// To the left and moving left
			// or
			// to the right and moving right 
			(npcVelocity < 0 && currentX <= leftTurnX) ||
			(npcVelocity > 0 && currentX >= rightTurnX)
		) {
			npcVelocity = -npcVelocity;
		}
		rb.linearVelocityX = npcVelocity;
	}

}
