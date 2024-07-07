using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreateAndDestroy : MonoBehaviourPunCallbacks
{
    [Header("Create")]
    public GameObject objectToCreate;
    public GameObject particleEffect;
    GameObject particle;
    public Transform spawnPoint;

    [Header("Destroy")]
    public Camera cam;
    public float range = 5f;
    public Vector3 raycastOffset = new Vector3(0, -0.1f, 0);
    public LayerMask destroyLayerMask;

    public bool isInCreateZone = false;
    public bool isInDestroyZone = false;
    private PhotonView view;
    public PickUpAndDrop pickUpAndDropScript;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        if (view == null)
        {
            Debug.LogError("PhotonView bile�eni bulunamad�!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Create"))
        {
            isInCreateZone = true;
        }
        else if (other.CompareTag("Destroy"))
        {
            isInDestroyZone = true;
            destroyLayerMask = pickUpAndDropScript.pickupLayerMask;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Create"))
        {
            isInCreateZone = false;
        }
        else if (other.CompareTag("Destroy"))
        {
            isInDestroyZone = false;
        }
    }

    public void CreateObject()
    {
        if (view.IsMine)
        {
            if (isInCreateZone && objectToCreate != null && spawnPoint != null)
            {
                Vector3 position = spawnPoint.position;
                Quaternion rotation = spawnPoint.rotation;
                GameObject newObject = PhotonNetwork.Instantiate(objectToCreate.name, position, rotation);
                PhotonView newObjectView = newObject.GetComponent<PhotonView>();
                if (newObjectView != null)
                {
                    pickUpAndDropScript.AddObjectToPickableList(newObjectView.ViewID); // ViewID'yi PickUpAndDrop scriptine g�nder
                }
                ShowParticleEffect(position);
                StartCoroutine(DestroyParticle());
                view.RPC("RPC_CreateObject", RpcTarget.OthersBuffered, newObjectView.ViewID, position, rotation);
            }
        }
    }

    [PunRPC]
    public void RPC_CreateObject(int viewID, Vector3 position, Quaternion rotation)
    {
        PhotonView newObjectView = PhotonView.Find(viewID);

        if (newObjectView == null)
        {
            GameObject newObject = PhotonNetwork.Instantiate(objectToCreate.name, position, rotation);
            newObjectView = newObject.GetComponent<PhotonView>();
        }
        ShowParticleEffect(position);
        StartCoroutine(DestroyParticle());

        if (newObjectView != null)
        {
            pickUpAndDropScript.AddObjectToPickableList(newObjectView.ViewID); // ViewID'yi PickUpAndDrop scriptine g�nder
        }
    }

    public void DestroyObject()
    {
        if(view.IsMine)
        {
            if (isInDestroyZone)
            {
                RaycastHit hit;
                Vector3 raycastOrigin = cam.transform.position + raycastOffset;

                if (Physics.Raycast(raycastOrigin, cam.transform.forward, out hit, range, destroyLayerMask))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    PhotonView objectPhotonView = hitObject.GetComponent<PhotonView>();

                    if (objectPhotonView != null && (objectPhotonView.IsMine || PhotonNetwork.IsMasterClient))
                    {
                        view.RPC("RPC_DestroyObject", RpcTarget.AllBuffered, objectPhotonView.ViewID);
                    }
                    else
                    {
                        Debug.LogWarning("Hedef nesne PhotonView bile�enine sahip de�il veya sahibi siz de�ilsiniz.");
                    }
                }
                else
                {
                    Debug.LogWarning("Raycast bir nesneye �arpmad�.");
                }
            }
        }
    }

    [PunRPC]
    public void RPC_DestroyObject(int objectViewID)
    {
        PhotonView targetView = PhotonView.Find(objectViewID);

        if (targetView != null)
        {
            GameObject obj = targetView.gameObject;

            if (obj != null)
            {
                ShowParticleEffect(obj.transform.position);
                PhotonNetwork.Destroy(obj);
                StartCoroutine(DestroyParticle());
                AddScore();
            }
            else
            {
                Debug.LogWarning("Belirtilen ViewID'ye sahip nesne bulunamad�.");
            }
        }
        else
        {
            Debug.LogWarning("Belirtilen ViewID'ye sahip PhotonView bulunamad�.");
        }
    }

    private void ShowParticleEffect(Vector3 position)
    {
        if (particleEffect != null)
        {
            particle = Instantiate(particleEffect, position, Quaternion.identity);
        }
    }

    private IEnumerator DestroyParticle()
    {
        yield return new WaitForSeconds(2f);
        Destroy(particle);
    }

    private void AddScore()
    {
        if (view.IsMine)
        {
            PlayerSetup player = GetComponent<PlayerSetup>();
            if (player != null)
            {
                player.AddScore(1);
            }
            else
            {
                Debug.LogWarning("Player bile�eni bulunamad�.");
            }
        }
    }
}
