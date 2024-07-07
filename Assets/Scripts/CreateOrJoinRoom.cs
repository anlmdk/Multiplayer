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
        // Oda ismi ve maksimum oyuncu say�s�n� belirtiyoruz
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
        Debug.Log("Oda olu�turuldu: " + roomName);
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
        Debug.Log("Odaya kat�l�n�yor: " + roomName);
    }
    public override void OnJoinedRoom()
    {
        isRoomJoined = true;

        PhotonNetwork.LoadLevel("Game");

        Debug.Log("Oyuna kat�ld�.");
    }
    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
        Debug.Log("Lobiden ��k�l�yor...");
        errorManager.ShowErrorMessage("Lobiden ��k�l�yor...");
    }
    public override void OnLeftLobby()
    {
        Debug.Log("Lobiden ��k�ld�.");
        PhotonNetwork.LoadLevel("MainMenu");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Oda olu�turulamad�: " + message);
        errorManager.ShowErrorMessage("Oda olu�turulamad�.");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Oda mevcut de�il." + message);
        errorManager.ShowErrorMessage("Oda mevcut de�il.");
    }
}
