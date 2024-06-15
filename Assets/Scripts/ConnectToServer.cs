using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public void ConnectServer()
    {
        // Photon server'a ba�lan

        PhotonNetwork.ConnectUsingSettings();

        Debug.Log("Server'a baglandi.");
    }
    public override void OnConnectedToMaster()
    {
        // Baglanti basarili ise lobiye katil

        PhotonNetwork.JoinLobby();

        Debug.Log("Lobiye katildi.");
    }
    public override void OnJoinedLobby()
    {
        // Lobiye baglanti basarili ise lobby sahnesini yukle

        SceneManager.LoadScene("Lobby");

        Debug.Log("Lobi sahnesine gecti.");
    }
}
