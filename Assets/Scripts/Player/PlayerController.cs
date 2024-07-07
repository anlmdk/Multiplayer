using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [Header("References")]
    private InputHandler inputHandler;
    private PlayerSetup setup;

    [Header("Components")]
    Rigidbody rb;
    public Animator anim;
    PhotonView view;
    Vector3 moveDirection;

    [Header("Settings")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float rotationSpeed = 4f;
    [SerializeField] private float jumpForce = 5f;

    [Header("InputAxis")]
    float horizontalInput;
    float verticalInput;

    [Header("Bools")]
    private  bool isGrounded = true;
    private bool isRunning;
    private bool isHandShaking;
    private bool isMovingBackwards;

    private void Start()
    {
        isGrounded = true;

        inputHandler = GetComponent<InputHandler>();
        setup = GetComponent<PlayerSetup>();
        rb = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            HandleJump();

            if (inputHandler.GetHandShake() && !isHandShaking)
            {
                HandleHandShake();
            }
        }
    }

    private void FixedUpdate()
    {
        if (view.IsMine && !isHandShaking)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        // Hareket input degerlerini alma
        Vector3 input = inputHandler.GetMovement();

        horizontalInput = input.x;
        verticalInput = input.y;

        moveDirection = setup.playerCamera.transform.forward * verticalInput;
        moveDirection += setup.playerCamera.transform.right * horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * walkSpeed;

        // Geri hareket kontrolu
        isMovingBackwards = verticalInput < 0;

        // Run
        isRunning = inputHandler.GetRun();

        // Karakterin hareketi
        Vector3 movement = moveDirection * (isRunning ? runSpeed : walkSpeed);
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // Karakterin saga ve sola donmesi
        HandleRotation();

        // Hareket animasyonu
        anim.SetFloat("Speed", moveDirection.magnitude);
        anim.SetBool("isRunning", isRunning);
    }

    public void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = setup.playerCamera.transform.forward * verticalInput;
        targetDirection += setup.playerCamera.transform.right * horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        // Geri giderken yonu tersine cevir
        if (isMovingBackwards)
        {
            targetDirection = -targetDirection;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    //Zıplama methodu
    private void HandleJump()
    {
        if (inputHandler.GetJump() && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    [PunRPC]
    public void SetHandleShakeAnimationRPC()
    {
        anim.SetTrigger("handShake");
    }

    // El sallama
    public void HandleHandShake()
    {
        isHandShaking = true;
        view.RPC("SetHandleShakeAnimationRPC", RpcTarget.All);
        StartCoroutine(EndHandShake());
    }

    private IEnumerator EndHandShake()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        isHandShaking = false;
    }

    public void OnGroundCollisionEnter()
    {
        isGrounded = true;
        anim.SetBool("isJumping", false);
    }

    public void OnGroundCollisionExit() 
    {
        isGrounded = false;
        anim.SetBool("isJumping", true);
    }
}
