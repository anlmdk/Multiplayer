using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }

    [Header("References")]
    [SerializeField] private InputHandler inputHandler;

    [Header("Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 90f;

    Rigidbody rb;
    Animator anim;

    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
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
}
