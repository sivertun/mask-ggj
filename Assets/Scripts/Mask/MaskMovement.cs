using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaskMovement : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    public Rigidbody2D rb;
    [SerializeField] private Vector2 throwVector;

    float catchCooldown = 0f;

    bool isAttached = true;

    public bool CanBeCaught => !isAttached && catchCooldown <= 0f;

    private Collider2D maskCollider;
    private Collider2D parentCollider;
    
    private InputAction throwAction;
    private void Start()
    {
        throwAction = InputSystem.actions.FindAction("Throw");
    }

    public void Throw(Vector2 throwVector, GameObject parent)
    {
        maskCollider = GetComponent<Collider2D>();
        parentCollider = parent.GetComponent<Collider2D>();
        Debug.Log(parentCollider);
        Physics2D.IgnoreCollision(maskCollider, parentCollider, true);
        catchCooldown = 0.25f;

        isAttached = false;
        playerController.removeControlledNPC();
        transform.SetParent(null);
        this.throwVector = throwVector;
        rb.simulated = true;
        rb.AddForce(throwVector);
    }

    public void Catch(GameObject parent)
    {
        PlayerController pc = this.gameObject.GetComponent<PlayerController>();
        pc.setCurrentlyControlledNPC(parent);

        isAttached = true;
        transform.SetParent(parent.transform);
        transform.localPosition = new Vector2(0f, 0.25f);
        transform.localRotation = Quaternion.identity;
        transform.localScale = new Vector3(1f, 0.5f, 1f);
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
        rb.linearVelocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttached)
        {
            if (throwAction.WasPressedThisFrame())
            {
                Throw(throwVector, playerController.getCurrentlyControlledNPC());
            }
        }
        else if (catchCooldown > 0)
        {
            catchCooldown -= Time.deltaTime;

            if (catchCooldown <= 0 && parentCollider != null)
            {
                Physics2D.IgnoreCollision(maskCollider, parentCollider, false);
                parentCollider = null;
            }
        }
    }
}
