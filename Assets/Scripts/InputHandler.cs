using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    public Craft craft;

    private InputAction movementAction;
    private InputAction jumpAction;
    private InputAction runAction;


    private InputAction craftMoveAction;
    private InputAction craftDestroyAction;

    private void Awake()
    {
        playerInput = new PlayerInput();

        movementAction = playerInput.Player.Controller;
        jumpAction = playerInput.Player.Jump;
        runAction = playerInput.Player.Sprint;


        //craftMoveAction = playerInput.Player.Craft.craft.MoveObject;
        //craftDestroyAction = playerInput.Player.Craft.craft.DestroyObject;


        // Input aksiyonlarý tanýmla

        craftMoveAction.performed += ctx => craft.MoveObject();
        craftDestroyAction.performed += ctx => craft.DestroyObject();
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();
        playerInput.Player.Craft.Enable();
    }
    private void OnDisable()
    {
        playerInput?.Player.Disable();
        playerInput.Player.Craft.Disable();
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
