using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float jumpPower = 15f;
    [SerializeField] private float jumpPowerCuttingRateUponRelease = 0.7f;
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private Vector2 groundCheckBox = new Vector2(1, 0.3f);
    [SerializeField] private float coyoteMaxTime = 0.08f;
    [SerializeField] private LayerMask groundLayer;
    private float coyoteCounter = 0f;

    [SerializeField] private GameObject currentlyControlledNPC = null;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction runAction;

    private Vector2 horizontalInput = Vector2.zero;
    private bool jumpOnNextOpportunity = false;
    private bool releaseJumpEarly = false;
    private bool releasedJump = true;

    public void setCurrentlyControlledNPC(GameObject npc)
    {
        if (currentlyControlledNPC)
        {
            currentlyControlledNPC.layer = LayerMask.NameToLayer("Default");
        }
        currentlyControlledNPC = npc;
        currentlyControlledNPC.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    public void removeControlledNPC()
    {
        if (currentlyControlledNPC)
        {
            currentlyControlledNPC.layer = LayerMask.NameToLayer("Default");
        }
        currentlyControlledNPC = null;
    }

    public bool CheckNPCGrounded()
    {
        if (Physics2D.BoxCast(currentlyControlledNPC.transform.position, groundCheckBox, 0, -currentlyControlledNPC.transform.up, groundCheckDistance))
        {
            return true;
        } else
        {
            return false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        runAction = InputSystem.actions.FindAction("Sprint");
        groundLayer = LayerMask.GetMask("Ground");

        if (currentlyControlledNPC)
        {
            currentlyControlledNPC.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentlyControlledNPC is null) return;
        horizontalInput = moveAction.ReadValue<Vector2>();
        bool holdingJump = jumpAction.ReadValue<float>() > 0 ? true : false;
        bool holdingRun = runAction.ReadValue<float>() > 0 ? true : false;

        // Handle jumping
        bool npcGrounded = CheckNPCGrounded();
        if (npcGrounded)
        {
            coyoteCounter = coyoteMaxTime;
        } else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (holdingJump && releasedJump)
        {
            jumpOnNextOpportunity = true;
            releaseJumpEarly = false;
            releasedJump = false;
        } else if (holdingJump == false && releasedJump == false)
        {
            jumpOnNextOpportunity = false;
            releaseJumpEarly = true;   
            releasedJump = true;
        }

        // Running logic
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
        Vector2 velocityToApply = new Vector2(horizontalInput.x * walkSpeed, rb.linearVelocity.y);
        
        if (jumpOnNextOpportunity == true && coyoteCounter > 0)
        {
            jumpOnNextOpportunity = false;
            coyoteCounter = 0;
            velocityToApply.y = jumpPower;
        } else if (releaseJumpEarly == true && coyoteCounter < 0 && rb.linearVelocity.y > 0)
        {
            releaseJumpEarly = false;
            velocityToApply *= new Vector2(0, jumpPowerCuttingRateUponRelease);
        }

        rb.linearVelocity = velocityToApply;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(currentlyControlledNPC.transform.position - currentlyControlledNPC.transform.up * groundCheckDistance, groundCheckBox);
    }
}
