using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 25f;

    [SerializeField] private GameObject currentlyControlledNPC = null;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction runAction;

    private Vector2 horizontalInput = Vector2.zero;

    public GameObject getCurrentlyControlledNPC()
    {
        return currentlyControlledNPC;
    }

    public void setCurrentlyControlledNPC(GameObject npc)
    {
        currentlyControlledNPC = npc;
    }

    public void removeControlledNPC()
    {
        currentlyControlledNPC = null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        runAction = InputSystem.actions.FindAction("Sprint");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentlyControlledNPC is null) return;
        horizontalInput = moveAction.ReadValue<Vector2>();
        bool holdingJump = jumpAction.ReadValue<float>() > 0 ? true : false;
        bool holdingRun = runAction.ReadValue<float>() > 0 ? true : false;

        if (holdingJump)
        {
            print("Jumping!");
        }

        if (holdingRun)
        {
            print("Running!");
        }
    }

    // FixedUpdate is called at a fixed interval and is used for physics operations
    void FixedUpdate()
    {
        if (currentlyControlledNPC is null) return;
        Rigidbody2D rb = currentlyControlledNPC.GetComponent<Rigidbody2D>();

        // Set the velocity for NPC
        rb.linearVelocity = new Vector2(horizontalInput.x * walkSpeed, rb.linearVelocity.y);
    }
}
