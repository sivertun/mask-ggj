using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction runAction;
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
        Vector2 movementVector = moveAction.ReadValue<Vector2>();
        bool holdingJump = jumpAction.ReadValue<float>() > 0 ? true : false;
        bool holdingRun = runAction.ReadValue<float>() > 0 ? true : false;

        if (movementVector.x > 0)
        {
            // Move right
            print("Moving right");
        }

        if (movementVector.x < 0)
        {
            // Move left
            print("Moving left");
        }

        if (holdingJump)
        {
            print("Jumping!");
        }

        if (holdingRun)
        {
            print("Running!");
        }
    }
}
