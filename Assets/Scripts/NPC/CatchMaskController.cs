using UnityEngine;

public class CatchMaskController : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mask"))
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            pc.setCurrentlyControlledNPC(this.gameObject);
        }
    }
}
