
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaskMovement : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    public Rigidbody2D rb;
    private Vector2 throwVector;

    private AudioSource[] audioSources;

    float catchCooldown = 0f;

    bool isAttached = true;
    public event Action<bool> OnAttachmentChanged; 
    public bool CanBeCaught => !isAttached;

    private Collider2D maskCollider;
    private Collider2D parentCollider;

    [Header("Throw")] 
    [SerializeField] private float minThrowForce = 5f;

    [SerializeField] private float maxThrowForce = 15f;
    [SerializeField] private float chargeTime = 1.5f;

    private float currentCharge;
    private bool isCharging;
    
    [Header("Trajectory")]
    [SerializeField] LineRenderer trajectoryLine;
    [SerializeField] int trajectoryPoints = 15;
    [SerializeField] float maxPreviewTime = 0.4f;
    [SerializeField] float timeStep = 0.05f;
    [SerializeField] Transform throwOrigin;
    
    
    private InputAction throwAction;
    private InputAction aimAction;
    
    private Camera Camera => Camera.main;
    
    private void Start()
    {
        throwAction = InputSystem.actions.FindAction("Throw");
        aimAction = InputSystem.actions.FindAction("Aim");
        rb = GetComponent<Rigidbody2D>();
	audioSources = GetComponents<AudioSource>();
    }

    public void Throw(Vector2 velocity, GameObject parent)
    {
        maskCollider = GetComponent<Collider2D>();
        parentCollider = parent.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(maskCollider, parentCollider, true);
        catchCooldown = 0.25f;

        PlayerAnimation animation = GetComponent<PlayerAnimation>();
        animation.Throw(parent);

        isAttached = false;
        OnAttachmentChanged?.Invoke(isAttached);
        playerController.removeControlledNPC();
        transform.SetParent(null);
        rb.simulated = true;
        rb.linearVelocity = velocity;
	audioSources[1].Play(0);
    }

    public void Catch(GameObject parent, Transform attachpoint)
    {
        if (catchCooldown > 0 && parentCollider != null && parentCollider.gameObject == parent) return;

        if (parentCollider != null)
        {
            Physics2D.IgnoreCollision(maskCollider, parentCollider, false);
            parentCollider = null;
        }

        PlayerController pc = this.gameObject.GetComponent<PlayerController>();
        pc.setCurrentlyControlledNPC(parent);

        isAttached = true;
        OnAttachmentChanged?.Invoke(isAttached);
        transform.SetParent(parent.transform);
        transform.SetLocalPositionAndRotation(attachpoint.localPosition, Quaternion.identity);
        transform.localScale = new Vector3(1f, 0.5f, 1f);
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
        rb.linearVelocity = Vector2.zero;
	audioSources[0].Play(0);
    }

    void Update()
    {
        if (isAttached)
        {
            // On press
            if (throwAction.WasPressedThisFrame())
            {
                isCharging = true;
                currentCharge = 0;
                trajectoryLine.enabled = true;
            }
            
            // while holding
            if (isCharging && throwAction.IsPressed())
            {
                throwVector = GetThrowVector2D();
                currentCharge += Time.deltaTime;
                currentCharge = Mathf.Clamp(currentCharge, 0f, chargeTime);
                
                float chargePercent = currentCharge / chargeTime;
                float force = Mathf.Lerp(minThrowForce, maxThrowForce, chargePercent);

                UpdateTrajectory(force);
            }
            
            // Release → throw
            if (isCharging && throwAction.WasReleasedThisFrame())
            {
                float chargePercent = currentCharge / chargeTime;
                float force = Mathf.Lerp(minThrowForce, maxThrowForce, chargePercent);
                throwVector = GetThrowVector2D();
                
                Rigidbody2D playerRb = playerController.getCurrentlyControlledNPC().GetComponent<Rigidbody2D>();

                Throw(throwVector * force + playerRb.linearVelocity, playerController.getCurrentlyControlledNPC());

                isCharging = false;
                trajectoryLine.enabled = false;
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
    
    Vector2 GetThrowVector2D()
    {
        Vector2 screenPos = aimAction.ReadValue<Vector2>();
        Vector2 worldPos = Camera.ScreenToWorldPoint(screenPos);

        return (worldPos - (Vector2)throwOrigin.position).normalized;
    }
    
    void UpdateTrajectory(float force)
    {
        Vector3 startPos = throwOrigin.position;
        Vector3 startVelocity = throwVector.normalized * force;
        
        int points = Mathf.CeilToInt(maxPreviewTime / timeStep);
        trajectoryLine.positionCount = points;

        for (int i = 0; i < points; i++)
        {
            float t = i * timeStep;

            Vector3 point =
                startPos +
                startVelocity * t +
                Physics.gravity * (0.5f * t * t);

            trajectoryLine.SetPosition(i, point);
        }
    }

}
