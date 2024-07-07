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
        // Oda ismi ve maksimum oyuncu sayisini belirtiyoruz
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
        Debug.Log("Oda olusturuldu: " + roomName);
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
        Debug.Log("Odaya katiliniyor: " + roomName);
    }

    public override void OnJoinedRoom()
    {
        isRoomJoined = true;

        PhotonNetwork.LoadLevel("Game");

        Debug.Log("Oyuna katildi.");
    }

    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();

        Debug.Log("Lobiden cikiliyor...");

        errorManager.ShowErrorMessage("Lobiden çýkýlýyor...");
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Lobiden cikildi.");

        PhotonNetwork.LoadLevel("MainMenu");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Oda olusturulamadi: " + message);

        errorManager.ShowErrorMessage("Oda oluþturulamadý.");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Oda mevcut degil." + message);

        errorManager.ShowErrorMessage("Oda mevcut deðil.");
    }
}
