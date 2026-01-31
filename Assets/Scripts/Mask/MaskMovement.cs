using UnityEngine;

public class MaskMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] private Vector2 throwVector;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(throwVector);
    }
}
