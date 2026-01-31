
using UnityEngine;
using UnityEngine.InputSystem;

public class MaskMovement : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    public Rigidbody2D rb;
    [SerializeField] private Vector2 throwVector;

    bool isAttached = true;
    
    private InputAction throwAction;
    private void Start()
    {
        throwAction = InputSystem.actions.FindAction("Throw");
    }

    public void Throw(Vector2 throwVector)
    {
        isAttached = false;
        playerController.removeControlledNPC();
        transform.SetParent(null);
        this.throwVector = throwVector;
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = true;
        rb.AddForce(throwVector);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttached) return;

        if (throwAction.WasPressedThisFrame())
        {
            Throw(throwVector);
        }
    }
}
