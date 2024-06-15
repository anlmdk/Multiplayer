using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction movementAction;

    private void Awake()
    {
        playerInput = new PlayerInput();

        movementAction = playerInput.Player.Controller;
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
}
