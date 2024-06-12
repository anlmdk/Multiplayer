using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateOrJoinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField createInput;

    [SerializeField] private TMP_InputField joinInput;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);

        Debug.Log("Oda olusturuldu.");
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);

        Debug.Log("Odaya katýldý.");
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");

        Debug.Log("Oyuna katýldý.");
    }
}
