using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; set; }

    [Header("References")]
    [SerializeField] private InputHandler inputHandler;

    [Header("Settings")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float rotationSpeed = 4f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Components")]
    Rigidbody rb;
    Animator anim;
    PhotonView view;

    [Header("Bools")]
    private bool isGrounded;
    private bool isRunning;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (view.IsMine)
        {
            Movement();
            Jump();
        }
    }

    private void Movement()
    {
        // Movement input 

        Vector3 input = inputHandler.GetMovement();

        // Hareket yönü

        Vector3 moveDirection = new Vector3(input.x, 0, input.y).normalized;

        // Karakterin saga ve sola donmesi

        if (moveDirection.magnitude > 0.1f)
        {
            float targetRotation = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            Quaternion targetRotationQuaternion = Quaternion.Euler(0, targetRotation, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotationQuaternion, rotationSpeed * Time.deltaTime);
        }

        // Koþma kontrolü
        isRunning = inputHandler.GetRun();

        // Hareket animasyonu
        anim.SetFloat("Speed", moveDirection.magnitude);
        anim.SetBool("isRunning", isRunning);

        // Hareket et
        Vector3 movement = moveDirection * (isRunning ? runSpeed : walkSpeed);
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }
    private void Jump()
    {
        // Zýplama kontrolü

        if (inputHandler.GetJump() && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;

            anim.SetBool("isJumping", true);
        }
    }
    public void OnGroundCollisionEnter()
    {
        isGrounded = true;
        anim.SetBool("isJumping", false);
    }
}
