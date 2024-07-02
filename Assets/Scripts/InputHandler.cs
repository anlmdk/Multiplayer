using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("References")]
    private PlayerInput playerInput;
    private Craft craft;

    private InputAction movementAction;
    private InputAction jumpAction;
    private InputAction runAction;

    private InputAction createObjectAction;
    private InputAction carryObjectAction;
    private InputAction destroyObjectAction;

    private InputAction handShakeAction;

    private void Awake()
    {
        playerInput = new PlayerInput();

        craft = GetComponent<Craft>();

        movementAction = playerInput.Player.Controller;
        jumpAction = playerInput.Player.Jump;
        runAction = playerInput.Player.Sprint;

        createObjectAction = playerInput.Player.Create;
        carryObjectAction = playerInput.Player.PickUpDrop;
        destroyObjectAction = playerInput.Player.Destroy;

        handShakeAction = playerInput.Player.HandShake;

        createObjectAction.performed += ctx => craft.CreateObject();
        carryObjectAction.started += ctx => craft.PickUp();
        carryObjectAction.canceled += ctx => craft.Drop();
        destroyObjectAction.performed += ctx => craft.DestroyObject();
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
    public bool GetHandShake()
    {
        return handShakeAction.triggered;
    }
}
