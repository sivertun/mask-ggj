using UnityEngine;

public class NPCMovement : MonoBehaviour
{
	[SerializeField] private int leftRange;
	[SerializeField] private int rightRange;
	[SerializeField] private int npcVelocity;
	public Rigidbody2D rb;
	private int startX;
	private int leftTurnX;
	private int rightTurnX;
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		startX = (int) rb.position.x;
		leftTurnX = startX - leftRange;
		rightTurnX = startX + rightRange;
	}
	void FixedUpdate()
	{
		int currentX = (int) rb.position.x;
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
