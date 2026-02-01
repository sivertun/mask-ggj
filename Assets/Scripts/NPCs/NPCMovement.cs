using UnityEngine;

public class NPCMovement : MonoBehaviour
{
	protected bool doMove = true;

	public void startMovement() {
		doMove = true;
	}
	public void stopMovement() {
		doMove = false;
	}
}
