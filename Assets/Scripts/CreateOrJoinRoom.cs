using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class CreateOrJoinRoom : MonoBehaviourPunCallbacks
{
    public static bool isRoomJoined = false;

    [SerializeField] private TMP_InputField createInput;

    [SerializeField] private TMP_InputField joinInput;

    [SerializeField] private int maxPlayers = 4;

    [SerializeField] private ErrorManager errorManager;

    public void CreateRoom()
    {
        // Oda ismi ve maksimum oyuncu sayýsýný belirtiyoruz
        string roomName = createInput.text;
        if (string.IsNullOrWhiteSpace(roomName))
        {
            Debug.Log("Oda ismi girilmedi.");
            errorManager.ShowErrorMessage("Oda ismi girilmedi.");
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
        Debug.Log("Oda oluþturuldu: " + roomName);
    }
    public void JoinRoom()
    {
        string roomName = joinInput.text;
        if (string.IsNullOrWhiteSpace(roomName))
        {
            Debug.Log("Oda ismi girilmedi.");
            errorManager.ShowErrorMessage("Oda ismi girilmedi.");
            return;
        }

        PhotonNetwork.JoinRoom(roomName);
        Debug.Log("Odaya katýlýnýyor: " + roomName);
    }
    public override void OnJoinedRoom()
    {
        isRoomJoined = true;

        PhotonNetwork.LoadLevel("Game");

        Debug.Log("Oyuna katýldý.");
    }
    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
        Debug.Log("Lobiden çýkýlýyor...");
        errorManager.ShowErrorMessage("Lobiden çýkýlýyor...");
    }
    public override void OnLeftLobby()
    {
        Debug.Log("Lobiden çýkýldý.");
        PhotonNetwork.LoadLevel("MainMenu");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Oda oluþturulamadý: " + message);
        errorManager.ShowErrorMessage("Oda oluþturulamadý.");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Oda mevcut deðil." + message);
        errorManager.ShowErrorMessage("Oda mevcut deðil.");
    }
}
