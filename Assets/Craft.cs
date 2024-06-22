using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    public GameObject objectToCreate; // Olusturulacak objenin prefab'i
    public GameObject particleEffect; // Kullanilacak partikul efektin prefab'i
    public Transform spawnPoint; // Objenin olusturulacagý nokta
    public Transform moveTarget; // Objenin tasinacagý hedef nokta

    private GameObject createdObject;

    void Start()
    {
        // Baslangiçta bir obje olustur

        CreateObject();
    }
    public void CreateObject()
    {
        if (objectToCreate != null && spawnPoint != null)
        {
            // PhotonNetwork ile obje olusturma

            createdObject = PhotonNetwork.Instantiate(objectToCreate.name, spawnPoint.position, spawnPoint.rotation);

            // Obje olusturuldugunda partikul efekti goster

            if (particleEffect != null)
            {
                Instantiate(particleEffect, spawnPoint.position, Quaternion.identity);
            }
        }
    }
    public void MoveObject()
    {
        if (createdObject != null && moveTarget != null)
        {
            // Sadece kendi objemizi taþý

            if (createdObject.GetComponent<PhotonView>().IsMine)
            {
                createdObject.transform.position = moveTarget.position;

                // Obje tasindiginda partikul efekti goster

                if (particleEffect != null)
                {
                    Instantiate(particleEffect, moveTarget.position, Quaternion.identity);
                }
            }
        }
    }
    public void DestroyObject()
    {
        if (createdObject != null)
        {
            // Sadece kendi objemizi yok et

            if (createdObject.GetComponent<PhotonView>().IsMine)
            {
                // Obje yok edilmeden önce partikul efekti goster

                if (particleEffect != null)
                {
                    Instantiate(particleEffect, createdObject.transform.position, Quaternion.identity);
                }
                PhotonNetwork.Destroy(createdObject);
            }
        }
    }
}
