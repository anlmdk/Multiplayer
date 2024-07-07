using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [Header("References")]
    public GameObject playerCamera;
    private GameUIManager gameUIManager;

    [Header("UI")]
    public int score = 0;
    private TMP_Text scoreText;
    private TMP_Text playerNameText;

    private void Start()
    {
        gameUIManager = FindObjectOfType<GameUIManager>();

        // Her oyuncunun kendi kamerasini aktif et
        if (photonView.IsMine)
        {
            playerCamera.SetActive(true);
        }
        else
        {
            playerCamera.SetActive(false);
        }
        
        // Baslangicta skoru ayarla
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

        // UI'deki skoru guncelle
        if (gameUIManager != null)
        {
            gameUIManager.UpdatePlayerScore(photonView.Owner.ActorNumber, score);
        }
    }
}
