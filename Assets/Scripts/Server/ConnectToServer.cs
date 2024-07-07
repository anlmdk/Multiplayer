using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // Photon server'a baglan
    public void ConnectServer()
    {
        PhotonNetwork.ConnectUsingSettings();

        Debug.Log("Server'a baglandi.");
    }

    // Baglanti basarili ise lobiye katil
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();

        Debug.Log("Lobiye katildi.");
    }

    // Lobiye baglanti basarili ise lobby sahnesini yukle
    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");

        Debug.Log("Lobi sahnesine gecti.");
    }
}
