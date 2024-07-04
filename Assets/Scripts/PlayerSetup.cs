using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public GameObject playerCamera;

    public int score = 0;
    public TMP_Text scoreText;
    public TMP_Text playerNameText;


    private void Start()
    {
        if (photonView.IsMine)
        {
            playerCamera.SetActive(true);
            playerNameText.text = PhotonNetwork.NickName;
        }
        else
        {
            playerCamera.SetActive(false);
        }
        UpdateScoreText();
    }
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
        photonView.RPC("UpdateScoreOnNetwork", RpcTarget.All, score, photonView.ViewID);
    }
    [PunRPC]
    public void UpdateScoreOnNetwork(int newScore, int viewID)
    {
        if (photonView.ViewID == viewID)
        {
            score = newScore;
            UpdateScoreText();
        }
    }
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
