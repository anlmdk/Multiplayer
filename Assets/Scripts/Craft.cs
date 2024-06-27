using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Craft : MonoBehaviour
{
    [Header("Create")]
    public GameObject objectToCreate; 
    public GameObject particleEffect; 
    public Transform spawnPoint;

    [Header("Pick Up & Drop")]
    public Camera cam;
    [SerializeField] private GameObject heldObject;
    public Transform playerHandTransform;
    public float range = 2f;
    public Vector3 raycastOffset = new Vector3(0, -0.5f, 0);
    public LayerMask pickupLayerMask;


    private void Update()
    {
        HoldObject();

    }
    public void CreateObject()
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
            }
        }
    }
    public void PickUp()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = cam.transform.position + raycastOffset; // Iþýný daha aþaðýdan göndermek için pozisyonu ayarla
        Debug.Log("Raycast gönderiliyor...");
        if (Physics.Raycast(raycastOrigin, cam.transform.forward, out hit, range, pickupLayerMask))
        {
            Debug.Log("Raycast çarptý: " + hit.collider.name);
            // Sadece taþýnabilir nesnelere raycast yap
            if (heldObject == null)
            {
                heldObject = hit.collider.gameObject; // Nesneyi raycast'in çarptýðý nesne olarak güncelle

                Debug.Log("Nesne alýndý: " + heldObject.name);
                heldObject.transform.SetParent(playerHandTransform); // Nesneyi playerHandTransform altýna ekle
                heldObject.transform.localPosition = Vector3.zero; // Nesnenin pozisyonunu sýfýrla
                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true; // Nesnenin fiziðini kapat
                    rb.useGravity = false; // Yer çekimini kapat
                }
            }
        }
        else
        {
            Debug.Log("Raycast hiçbir þeye çarpmadý.");
        }
    }
    public void Drop()
    {
        if (heldObject != null)
        {
            // Obje býrakma iþlemi
            heldObject.transform.SetParent(null);
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Nesnenin fiziðini aç
                rb.useGravity = true; // Yer çekimini aç
            }
            heldObject = null;
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
    public void DestroyObject()
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
                }
            }
        }
    }
    private void HoldObject()
    {
        // E tuþuna basýldýðýnda nesneyi al
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                CreateObject();
            }
            PickUp();
        }
        // E tuþunu býraktýðýnda nesneyi býrak
        if (Input.GetKeyUp(KeyCode.E))
        {
            Drop();
        }
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
            Instantiate(particleEffect, position, Quaternion.identity);
        }
    }
}
