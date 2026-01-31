using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaskMovement : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    public Rigidbody2D rb;
    [SerializeField] private Vector2 throwVector;

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
    [SerializeField] float timeStep = 0.05f;
    [SerializeField] Transform throwOrigin;
    
    
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
    
    void Update()
    {
        if (!isAttached) return;

        if (throwAction.WasPressedThisFrame())
        {
            /*isCharging = true;
            currentCharge = 0;
            trajectoryLine.enabled = true;*/
            Throw(throwVector);
        }
    }
}
