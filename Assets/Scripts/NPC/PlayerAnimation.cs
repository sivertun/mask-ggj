
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer maskSpriteRenderer;
    private SpriteRenderer npcSpriteRenderer;
    
    private InputAction moveAction;
    private InputAction runAction;
    
    private PlayerController playerController;
    private Animator animator;
    GameObject controlledNPC;
    
    void Start()
    {
        runAction = InputSystem.actions.FindAction("Sprint");
        moveAction = InputSystem.actions.FindAction("Move");
        playerController = GetComponent<PlayerController>();
    }

    public void Throw(GameObject parent)
    {
        parent.GetComponent<Animator>();
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }
    
    void Update()
    {
        controlledNPC = playerController.getCurrentlyControlledNPC();
        
        if (!controlledNPC) return;
        
        animator = controlledNPC.GetComponent<Animator>();
        
        // Handle animating movement
        Vector2 horizontalInput = moveAction.ReadValue<Vector2>();
        bool holdingRun = runAction.ReadValue<float>() > 0 ? true : false;
        
        
        bool isMoving = Mathf.Abs(horizontalInput.x) > 0.01f;
        animator.SetBool("isWalking", isMoving);
        animator.SetBool("isRunning", holdingRun && isMoving);

        // Flip character
        if (horizontalInput.x > 0)
            FaceRight();
        else if (horizontalInput.x < 0)
            FaceLeft();
    }
    
    void FaceRight()
    {
        if (!transform.parent) return;
        Vector3 scale = transform.parent.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.parent.localScale = scale;
    }

    void FaceLeft()
    {
        if (!transform.parent) return;
        Vector3 scale = transform.parent.localScale;
        scale.x = -Mathf.Abs(scale.x);
        transform.parent.localScale = scale;
    }
}
