using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public GameObject playerCamera;
    private GameUIManager gameUIManager;

    public int score = 0;
    private TMP_Text scoreText;
    private TMP_Text playerNameText;


    private void Start()
    {
        gameUIManager = FindObjectOfType<GameUIManager>();

        if (photonView.IsMine)
        {
            playerCamera.SetActive(true);
        }
        else
        {
            playerCamera.SetActive(false);
        }
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        photonView.RPC("RPC_AddScore", RpcTarget.All, amount);
    }

    [PunRPC]
    public void RPC_AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();

        photonView.RPC("UpdateScoreOnNetwork", RpcTarget.All, score, photonView.Owner.ActorNumber);
    }

    [PunRPC]
    public void UpdateScoreOnNetwork(int newScore, int actorNumber)
    {
        if (photonView.Owner.ActorNumber == actorNumber)
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
        // UI'deki skoru güncelle
        if (gameUIManager != null)
        {
            gameUIManager.UpdatePlayerScore(photonView.Owner.ActorNumber, score);
        }
    }
}
