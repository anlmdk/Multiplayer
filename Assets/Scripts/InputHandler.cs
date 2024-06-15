using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction movementAction;
    private InputAction jumpAction;
    private InputAction runAction;

    private void Awake()
    {
        playerInput = new PlayerInput();

        movementAction = playerInput.Player.Controller;
        jumpAction = playerInput.Player.Jump;
        runAction = playerInput.Player.Sprint;
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();
    }
    private void OnDisable()
    {
        playerInput?.Player.Disable();
    }

    public Vector2 GetMovement()
    {
        return movementAction.ReadValue<Vector2>();
    }
    public bool GetJump()
    {
        return jumpAction.triggered;
    }
    public bool GetRun()
    {
        return runAction.ReadValue<float>() > 0.1f;
    }
}
