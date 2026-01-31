using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaskMovement : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    public Rigidbody2D rb;
    private Vector2 throwVector;

    bool isAttached = true;


    [Header("Throw")] 
    [SerializeField] private float minThrowForce = 5f;

    [SerializeField] private float maxThrowForce = 20f;
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
    }

    public void Throw(Vector2 velocity)
    {
        isAttached = false;
        playerController.removeControlledNPC();
        transform.SetParent(null);
        rb.simulated = true;
        rb.linearVelocity = velocity;
    }
    
    void Update()
    {
        if (!isAttached) return;
    
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
            Throw(throwVector * force);

            isCharging = false;
            trajectoryLine.enabled = false;
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
