using UnityEngine;

public class PlayerInteractive : MonoBehaviour
{
    [Header("References")]
    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
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
