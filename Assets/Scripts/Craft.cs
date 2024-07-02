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
            Debug.LogError("PhotonView bileþeni bulunamadý!");
        }
    }
    public void CreateObject()
    {
        if (view.IsMine)
        {
            // Eðer þu anda bir nesne taþýyorsak veya raycast bir nesneye çarpýyorsa, yeni obje oluþturma
            if (heldObject == null && !IsRaycastHittingObject())
            {
                if (objectToCreate != null && spawnPoint != null)
                {
                    // Obje oluþturma iþlemi
                    GameObject newObject = PhotonNetwork.Instantiate(objectToCreate.name, spawnPoint.position, spawnPoint.rotation);

                    // Obje oluþturulduðunda partikül efektini göster
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
            Debug.Log("Raycast gönderiliyor...");
            if (Physics.Raycast(raycastOrigin, cam.transform.forward, out hit, range, pickupLayerMask))
            {
                Debug.Log("Raycast çarptý: " + hit.collider.name);

                if (heldObject == null)
                {
                    view.RPC("SetIsPickingUpAnimationRPC", RpcTarget.All);
                    heldObject = hit.collider.gameObject;

                    Debug.Log("Nesne alýndý: " + heldObject.name);
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
                Debug.Log("Raycast hiçbir þeye çarpmadý.");
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
                        // Obje yok edilmeden önce partikül efektini göster
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
        // Karakterin gözünün olduðu nokta
        Vector3 characterEyePosition = cam.transform.position + raycastOffset;

        // Karakterin gözünün baktýðý yön
        Vector3 characterEyeForwardDirection = cam.transform.forward;

        // Raycast'ýn rengini ayarla (örneðin kýrmýzý)
        Gizmos.color = Color.red;

        // Raycast'ý çiz
        Gizmos.DrawRay(characterEyePosition, characterEyeForwardDirection * range);
    }
    private IEnumerator DestroyParticle()
    {
        yield return new WaitForSeconds(2f);
        Destroy(particle);
    }
}
