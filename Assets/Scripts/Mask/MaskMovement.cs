using UnityEngine;

public class MaskMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] private Vector2 throwVector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void Initialize(Vector2 throwVector)
    {
        this.throwVector = throwVector;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(throwVector);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
