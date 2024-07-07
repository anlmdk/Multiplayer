using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class GameUIManager : MonoBehaviourPunCallbacks
{
    public Transform gamePanel;

    private static int playerCount = 0;

    // Oyuncularin datasini saklayabilmek icin sozluk olustur
    private Dictionary<int, GameObject> playerUIDictionary = new Dictionary<int, GameObject>();

    float xPosition;

    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                CreatePlayerUI(player.ActorNumber, player.NickName);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        CreatePlayerUI(newPlayer.ActorNumber, newPlayer.NickName);
    }

    public void CreatePlayerUI(int playerID, string playerName)
    {
        if (playerUIDictionary.ContainsKey(playerID))
        {
            Debug.LogWarning($"Player UI player icin {playerID} zaten olusturulmus.");
            return;
        }

        GameObject playerUIInstance = Resources.Load<GameObject>("PlayerUI");

        if (playerUIInstance == null)
        {
            Debug.LogError("Player UI Resources dosyasi uzerinden yuklenemedi.");
            return;
        }

        if (gamePanel == null)
        {
            Debug.LogError("GamePanel atanmamis.");
            return;
        }

        GameObject playerUI = Instantiate(playerUIInstance, gamePanel);
        RectTransform rectTransform = playerUI.GetComponent<RectTransform>();

        if (playerCount == 0)
        {
            rectTransform.anchoredPosition = new Vector2(200, -100);
            xPosition = rectTransform.anchoredPosition.x;
        }
        else
        {
            rectTransform.anchoredPosition = new Vector2(xPosition + 200, -100);
        }

        rectTransform.sizeDelta = new Vector2(200, 50);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        TMP_Text playerIDText = playerUI.transform.Find("PlayerIDText").GetComponent<TMP_Text>();
        TMP_Text scoreText = playerUI.transform.Find("PlayerIDText/ScoreText").GetComponent<TMP_Text>();

        if (playerIDText == null)
        {
            Debug.LogError("PlayerIDText bileseni playerUI prefabinda bulunamadi.");
            return;
        }
        if (scoreText == null)
        {
            Debug.LogError("ScoreText bileseni playerUI prefabinda bulunamadi.");
            return;
        }

        playerIDText.text = "Player " + playerID; 
        scoreText.text = "0";

        playerUIDictionary.Add(playerID, playerUI);
        playerCount++;
    }

    public void UpdatePlayerScore(int playerID, int score)
    {
        if (playerUIDictionary.ContainsKey(playerID))
        {
            GameObject playerUI = playerUIDictionary[playerID];
            TMP_Text scoreText = playerUI.transform.Find("PlayerIDText/ScoreText").GetComponent<TMP_Text>();

            if (scoreText != null)
            {
                scoreText.text = score.ToString();
            }
        }
    }
}
