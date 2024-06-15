using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }

    [Header("References")]
    [SerializeField] private InputHandler inputHandler;

    [Header("Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float rotationSpeed = 90f;
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
            Run();
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
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationQuaternion, rotationSpeed * Time.deltaTime);
        }

        // Karakterin hareketi

        Vector3 movement = moveDirection * speed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);


        // Karakterin x ve y'deki hareketi ve hiza gore hareket animasyonu

        anim.SetFloat("Speed", rb.velocity.sqrMagnitude);
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
    private void Run()
    {
        // Joystick ile kosma kontrolu

        if (inputHandler.GetMovement().magnitude > 0.1f) 
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        // Klavye ile kosma kontrolu (Shift tuþu)

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Karakter yere carptiginda ziplama iznini geri ver

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            anim.SetBool("isJumping", false);
        }
    }
}
