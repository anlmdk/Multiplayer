using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractive : MonoBehaviour
{
    [Header("References")]
    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController bileþeni bulunamadý!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Karakter yere carptiginda ziplama iznini geri ver

        if (collision.gameObject.CompareTag("Ground"))
        {
            if (playerController != null)
            {
                playerController.OnGroundCollisionEnter();
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (playerController != null)
            {
                playerController.OnGroundCollisionExit();
            }
        }
    }
}
