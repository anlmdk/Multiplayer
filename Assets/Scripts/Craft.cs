using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviourPunCallbacks
{
    [Header("References")]
    PhotonView view;
    private PlayerController playerController;

    [Header("Create")]
    public GameObject objectToCreate; 
    public GameObject particleEffect;
    GameObject particle;
    public Transform spawnPoint;
    

    [Header("Pick Up & Drop")]
    public Camera cam;
    [SerializeField] private GameObject heldObject;
    public Transform playerHandTransform;
    public float range = 5f;
    public Vector3 raycastOffset = new Vector3(0, -0.1f, 0);
    public LayerMask pickupLayerMask;


    private void Start()
    {
        view = GetComponent<PhotonView>();
        playerController = GetComponent<PlayerController>();

        if (view == null)
        {
            Debug.LogError("PhotonView bile�eni bulunamad�!");
        }
    }
    public void CreateObject()
    {
        if (view.IsMine)
        {
            // E�er �u anda bir nesne ta��yorsak veya raycast bir nesneye �arp�yorsa, yeni obje olu�turma
            if (heldObject == null && !IsRaycastHittingObject())
            {
                if (objectToCreate != null && spawnPoint != null)
                {
                    // Obje olu�turma i�lemi
                    GameObject newObject = PhotonNetwork.Instantiate(objectToCreate.name, spawnPoint.position, spawnPoint.rotation);

                    // Obje olu�turuldu�unda partik�l efektini g�ster
                    ShowParticleEffect(spawnPoint.position);

                    StartCoroutine(DestroyParticle());
                }
            }
        }
    }
    public void PickUp()
    {
        if(view.IsMine)
        {
            RaycastHit hit;
            Vector3 raycastOrigin = cam.transform.position + raycastOffset;
            Debug.Log("Raycast g�nderiliyor...");
            if (Physics.Raycast(raycastOrigin, cam.transform.forward, out hit, range, pickupLayerMask))
            {
                Debug.Log("Raycast �arpt�: " + hit.collider.name);

                if (heldObject == null)
                {
                    view.RPC("SetIsPickingUpAnimationRPC", RpcTarget.All);
                    heldObject = hit.collider.gameObject;

                    Debug.Log("Nesne al�nd�: " + heldObject.name);
                    heldObject.transform.SetParent(playerHandTransform);
                    heldObject.transform.localPosition = Vector3.zero;
                    Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                        rb.useGravity = false;
                    }
                }
            }
            else
            {
                Debug.Log("Raycast hi�bir �eye �arpmad�.");
            }
        }
    }
    public void Drop()
    {
        if (view.IsMine)
        {
            if (heldObject != null)
            {
                view.RPC("SetIsPickingUpAnimationRPC", RpcTarget.All);

                heldObject.transform.SetParent(null);
                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                }
                heldObject = null;
            }
        }
    }
    public void DestroyObject()
    {
        if(view.IsMine)
        {
            RaycastHit hit;
            Vector3 raycastOrigin = cam.transform.position + raycastOffset;
            if (Physics.Raycast(raycastOrigin, cam.transform.forward, out hit, range, pickupLayerMask))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject != heldObject)
                {
                    Debug.Log("Raycast ile vurulan nesne: " + hitObject.name);

                    // PhotonNetwork ile sadece kendi objemizi kontrol ederek yok ediyoruz
                    if (hitObject.GetComponent<PhotonView>().IsMine)
                    {
                        // Obje yok edilmeden �nce partik�l efektini g�ster
                        ShowParticleEffect(hitObject.transform.position);

                        // PhotonNetwork ile objeyi yok et
                        PhotonNetwork.Destroy(hitObject);

                        StartCoroutine(DestroyParticle());
                    }
                }
            }
        }
    }
    [PunRPC]
    public void SetIsPickingUpAnimationRPC()
    {
        playerController.anim.SetTrigger("isPickingUp");
    }
    private bool IsRaycastHittingObject()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = cam.transform.position + raycastOffset;
        return Physics.Raycast(raycastOrigin, cam.transform.forward, out hit, range, pickupLayerMask);
    }
    private void ShowParticleEffect(Vector3 position)
    {
        if (particleEffect != null)
        {
            particle = Instantiate(particleEffect, position, Quaternion.identity);
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Karakterin g�z�n�n oldu�u nokta
        Vector3 characterEyePosition = cam.transform.position + raycastOffset;

        // Karakterin g�z�n�n bakt��� y�n
        Vector3 characterEyeForwardDirection = cam.transform.forward;

        // Raycast'�n rengini ayarla (�rne�in k�rm�z�)
        Gizmos.color = Color.red;

        // Raycast'� �iz
        Gizmos.DrawRay(characterEyePosition, characterEyeForwardDirection * range);
    }
    private IEnumerator DestroyParticle()
    {
        yield return new WaitForSeconds(2f);
        Destroy(particle);
    }
}
