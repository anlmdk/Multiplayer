using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAndDrop : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("References")]
    PhotonView view;

    [Header("Pick Up & Drop")]
    public Camera cam;
    public GameObject heldObject;
    public Transform playerHandTransform;
    public float range = 5f;
    public Vector3 raycastOffset = new Vector3(0, -0.1f, 0);
    public LayerMask pickupLayerMask;
    private bool isDragging = false;
    private List<int> pickableObjects = new List<int>();

    private void Start()
    {
        view = GetComponent<PhotonView>();

        if (view == null)
        {
            Debug.LogError("PhotonView bileþeni bulunamadý!");
        }
    }

    private void Update()
    {
        if (view.IsMine)
        {
            if (isDragging && heldObject != null)
            {
                DragObject();
            }
        }
    }

    public void PickUp()
    {
        if (view.IsMine)
        {
            RaycastHit hit;
            Vector3 raycastOrigin = cam.transform.position + raycastOffset;
            if (Physics.Raycast(raycastOrigin, cam.transform.forward, out hit, range, pickupLayerMask))
            {
                if (heldObject == null)
                {
                    PhotonView objectPhotonView = hit.collider.gameObject.GetComponent<PhotonView>();

                    if (objectPhotonView != null && pickableObjects.Contains(objectPhotonView.ViewID))
                    {
                        view.RPC("RPC_PickUp", RpcTarget.AllBuffered, objectPhotonView.ViewID);
                    }
                    else
                    {
                        Debug.LogWarning("Nesne PhotonView bileþenine sahip deðil.");
                    }
                }
            }
        }
    }

    [PunRPC]
    public void RPC_PickUp(int objectViewID)
    {
        PhotonView objectPhotonView = PhotonNetwork.GetPhotonView(objectViewID);

        Debug.Log(objectPhotonView);

        if (objectPhotonView != null)
        {
            GameObject obj = objectPhotonView.gameObject;
            heldObject = obj;
            heldObject.transform.SetParent(playerHandTransform);
            heldObject.transform.localPosition = Vector3.zero;
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
            isDragging = true;
        }
        else
        {
            Debug.LogWarning("Belirtilen ViewID'ye sahip nesne bulunamadý.");
        }
    }

    public void Drop()
    {
        if(view.IsMine)
        {
            if (heldObject != null)
            {
                view.RPC("RPC_Drop", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    public void RPC_Drop()
    {
        if (heldObject != null)
        {
            heldObject.transform.SetParent(null);
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            heldObject = null;
            isDragging = false;
        }
    }

    private void DragObject()
    {
        heldObject.transform.position = playerHandTransform.position;
    }

    public bool IsRaycastHittingObject()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = cam.transform.position + raycastOffset;
        return Physics.Raycast(raycastOrigin, cam.transform.forward, out hit, range, pickupLayerMask);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 characterEyePosition = cam.transform.position + raycastOffset;
        Vector3 characterEyeForwardDirection = cam.transform.forward;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(characterEyePosition, characterEyeForwardDirection * range);
    }
    public void AddObjectToPickableList(int viewID) // Bu metodu ekleyin
    {
        if (!pickableObjects.Contains(viewID))
        {
            pickableObjects.Add(viewID);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // Bu metodu ekleyin
    {
        if (stream.IsWriting)
        {
            if (isDragging && heldObject != null)
            {
                stream.SendNext(heldObject.transform.position);
                stream.SendNext(heldObject.transform.rotation);
            }
        }
        else
        {
            if (isDragging && heldObject != null)
            {
                heldObject.transform.position = (Vector3)stream.ReceiveNext();
                heldObject.transform.rotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}

