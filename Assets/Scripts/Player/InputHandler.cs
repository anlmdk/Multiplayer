using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("References")]
    private PlayerInput playerInput;
    private PickUpAndDrop pickUpAndDrop;
    private CreateAndDestroy createAndDestroy;

    [Header("MovementActions")]
    private InputAction movementAction;
    private InputAction jumpAction;
    private InputAction runAction;

    [Header("CreateActions")]
    private InputAction createObjectAction;
    private InputAction pickUpOrDropObjectAction;
    private InputAction destroyObjectAction;

    [Header("EnjoyableActions")]
    private InputAction handShakeAction;

    private bool isPickUpOrDropObject = false;

    private void Awake()
    {
        playerInput = new PlayerInput();

        createAndDestroy = GetComponent<CreateAndDestroy>();
        pickUpAndDrop = GetComponent<PickUpAndDrop>();

        movementAction = playerInput.Player.Controller;
        jumpAction = playerInput.Player.Jump;
        runAction = playerInput.Player.Sprint;

        createObjectAction = playerInput.Player.Create;
        pickUpOrDropObjectAction = playerInput.Player.PickUpDrop;
        destroyObjectAction = playerInput.Player.Destroy;

        handShakeAction = playerInput.Player.HandShake;

        createObjectAction.performed += ctx => createAndDestroy.CreateObject();
        destroyObjectAction.performed += ctx => createAndDestroy.DestroyObject();
        pickUpOrDropObjectAction.performed += ctx =>
        {
            if (!isPickUpOrDropObject)
            {
                pickUpAndDrop.PickUp();
                isPickUpOrDropObject= true;
            }
            else
            {
                pickUpAndDrop.Drop();
                isPickUpOrDropObject = false;
            }
        };
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
