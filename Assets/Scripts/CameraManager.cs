using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;

    private Vector3 camFollowVelocity = Vector3.zero;

    public float camFollowSpeed = 0.3f;

    private void Awake()
    {
        while (player == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                if (PhotonView.Get(player).IsMine)
                {
                    this.player = player.transform;
                    break;
                }
            }
        }
    }
    private void LateUpdate()
    {
        if (player != null)
        {
            FollowTarget();
        }
    }
    public void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, player.position, ref camFollowVelocity, camFollowSpeed);

        transform.position = targetPosition;
    }
}
