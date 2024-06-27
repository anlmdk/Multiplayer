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
        // E�er �u anda bir nesne ta��yorsak veya raycast bir nesneye �arp�yorsa, yeni obje olu�turma
        if (heldObject == null && !IsRaycastHittingObject())
        {
            if (objectToCreate != null && spawnPoint != null)
            {
                // Obje olu�turma i�lemi
                GameObject newObject = PhotonNetwork.Instantiate(objectToCreate.name, spawnPoint.position, spawnPoint.rotation);

                // Obje olu�turuldu�unda partik�l efektini g�ster
                ShowParticleEffect(spawnPoint.position);
            }
        }
    }
    public void PickUp()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = cam.transform.position + raycastOffset; // I��n� daha a�a��dan g�ndermek i�in pozisyonu ayarla
        Debug.Log("Raycast g�nderiliyor...");
        if (Physics.Raycast(raycastOrigin, cam.transform.forward, out hit, range, pickupLayerMask))
        {
            Debug.Log("Raycast �arpt�: " + hit.collider.name);
            // Sadece ta��nabilir nesnelere raycast yap
            if (heldObject == null)
            {
                heldObject = hit.collider.gameObject; // Nesneyi raycast'in �arpt��� nesne olarak g�ncelle

                Debug.Log("Nesne al�nd�: " + heldObject.name);
                heldObject.transform.SetParent(playerHandTransform); // Nesneyi playerHandTransform alt�na ekle
                heldObject.transform.localPosition = Vector3.zero; // Nesnenin pozisyonunu s�f�rla
                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true; // Nesnenin fizi�ini kapat
                    rb.useGravity = false; // Yer �ekimini kapat
                }
            }
        }
        else
        {
            Debug.Log("Raycast hi�bir �eye �arpmad�.");
        }
    }
    public void Drop()
    {
        if (heldObject != null)
        {
            // Obje b�rakma i�lemi
            heldObject.transform.SetParent(null);
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Nesnenin fizi�ini a�
                rb.useGravity = true; // Yer �ekimini a�
            }
            heldObject = null;
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
                    // Obje yok edilmeden �nce partik�l efektini g�ster
                    ShowParticleEffect(hitObject.transform.position);

                    // PhotonNetwork ile objeyi yok et
                    PhotonNetwork.Destroy(hitObject);
                }
            }
        }
    }
    private void HoldObject()
    {
        // E tu�una bas�ld���nda nesneyi al
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                CreateObject();
            }
            PickUp();
        }
        // E tu�unu b�rakt���nda nesneyi b�rak
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
