using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractive : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Karakter yere carptiginda ziplama iznini geri ver

        if (collision.gameObject.CompareTag("Ground"))
        {
            PlayerController.instance.OnGroundCollisionEnter();
        }
    }
}
