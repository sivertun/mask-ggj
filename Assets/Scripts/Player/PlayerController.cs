using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float deceleration = 100f;
    [SerializeField] private float maxWalkSpeed = 10f;
    [SerializeField] private float maxRunSpeed = 18f;
    [SerializeField] private float boostCutoff = 3f;
    [SerializeField] private float walkAcceleration = 50f;
    [SerializeField] private float runAcceleration = 80f;
    [SerializeField] private float jumpPower = 15f;
    [SerializeField] private float jumpPowerCuttingRateUponRelease = 0.7f;
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private Vector2 groundCheckBox = new Vector2(1, 0.3f);
    [SerializeField] private float coyoteMaxTime = 0.08f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private PhysicsMaterial2D originalMaterial;
    [SerializeField] private PhysicsMaterial2D zeroFrictionMaterial;
    private float coyoteCounter = 0f;

    [SerializeField] private GameObject currentlyControlledNPC = null;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction runAction;

    private Vector2 horizontalInput = Vector2.zero;
    private bool jumpOnNextOpportunity = false;
    private bool releaseJumpEarly = false;
    private bool releasedJump = true;
    private bool running = false;

    public GameObject getCurrentlyControlledNPC()
    {
        return currentlyControlledNPC;
    }

    public void setCurrentlyControlledNPC(GameObject npc)
    {
        if (currentlyControlledNPC)
        {
            currentlyControlledNPC.layer = LayerMask.NameToLayer("Default");
        }
        currentlyControlledNPC = npc;
        currentlyControlledNPC.layer = LayerMask.NameToLayer("Ignore Raycast");

        var collider = currentlyControlledNPC.GetComponent<Collider2D>();
        originalMaterial = collider.sharedMaterial;
        collider.sharedMaterial = zeroFrictionMaterial;
    }

    public void removeControlledNPC()
    {
        if (currentlyControlledNPC)
        {
            currentlyControlledNPC.layer = LayerMask.NameToLayer("Default");

            Collider2D collider = currentlyControlledNPC.GetComponent<Collider2D>();
            collider.sharedMaterial = originalMaterial;
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

        zeroFrictionMaterial = new PhysicsMaterial2D();
        zeroFrictionMaterial.friction = 0f;
        zeroFrictionMaterial.frictionCombine = PhysicsMaterialCombine2D.Minimum;
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
        if (holdingRun)
        {
            running = true;
        } else
        {
            running = false;
        }
    }

    // FixedUpdate is called at a fixed interval and is used for physics operations
    void FixedUpdate()
    {
        if (currentlyControlledNPC is null) return;

        Rigidbody2D rb = currentlyControlledNPC.GetComponent<Rigidbody2D>();

        // Set the velocity for NPC
        Vector2 velocityToApply = new Vector2(rb.linearVelocityX, rb.linearVelocityY);

        float maxSpeed = running ? maxRunSpeed : maxWalkSpeed;
        float acceleration = running ? runAcceleration : walkAcceleration;

        float inputX = horizontalInput.x;
        float currentSpeed = rb.linearVelocityX;

        float targetSpeed = inputX * maxSpeed;

        bool boost = Mathf.Abs(currentSpeed) < boostCutoff;

        float newSpeed;

        if (inputX == 0)
        {
            float totalChange = deceleration * Time.fixedDeltaTime;
            if (currentSpeed > 0) newSpeed = Mathf.Max(currentSpeed - totalChange, 0);
            else if (currentSpeed < 0) newSpeed = Mathf.Min(currentSpeed + totalChange, 0);
            else newSpeed = 0;
        }
        else
        {
            float totalAcceleration;
            if (!boost && Mathf.Sign(inputX) == Mathf.Sign(currentSpeed)) totalAcceleration = acceleration;
            else totalAcceleration = acceleration + deceleration;

            float direction = Mathf.Sign(targetSpeed - currentSpeed);
            float totalChange = totalAcceleration * direction * Time.fixedDeltaTime;
            newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, totalAcceleration * Time.fixedDeltaTime);
        }

        velocityToApply.x = newSpeed;

        if (jumpOnNextOpportunity == true && coyoteCounter > 0)
        {
            jumpOnNextOpportunity = false;
            coyoteCounter = 0;
            velocityToApply.y = jumpPower;
        }
        else if (releaseJumpEarly == true && coyoteCounter < 0 && rb.linearVelocity.y > 0)
        {
            releaseJumpEarly = false;
            velocityToApply.y *= jumpPowerCuttingRateUponRelease;
        }

        rb.linearVelocity = velocityToApply;
    }

    private void OnDrawGizmos()
    {
        if (!currentlyControlledNPC) return;
        Gizmos.DrawWireCube(currentlyControlledNPC.transform.position - currentlyControlledNPC.transform.up * groundCheckDistance, groundCheckBox);
    }
}
