using UnityEngine;

public class CatchMaskController : MonoBehaviour
{
    [SerializeField] private Transform catchPoint;
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mask"))
        {
            CatchMask(collision.gameObject);
        }
    }

    void CatchMask(GameObject mask)
    {
        MaskMovement mm = mask.GetComponent<MaskMovement>();
        if (mm.CanBeCaught)
        {
            mm.Catch(this.gameObject, catchPoint);
        }
    }
}
