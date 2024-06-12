using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // Photon server'a baðlan

        PhotonNetwork.ConnectUsingSettings();

        Debug.Log("Server'a baglandi.");
    }
    public override void OnConnectedToMaster()
    {
        // Baglanti basarili ise lobiye katil

        PhotonNetwork.JoinLobby();

        Debug.Log("Lobiye katýldý.");
    }
    public override void OnJoinedLobby()
    {
        // Lobiye baglanti basarili ise lobby sahnesini yukle

        SceneManager.LoadScene("Lobby");

        Debug.Log("Lobi sahnesine gecti.");
    }
}
