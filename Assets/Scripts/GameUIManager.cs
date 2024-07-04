using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class GameUIManager : MonoBehaviourPunCallbacks
{
    public Transform gamePanel;
    private static int playerCount = 0;

    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                AddPlayerUI(player);
            }
        }
        // UI pozisyonunu ayarla
        RectTransform rectTransform = gamePanel.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(200, -100);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddPlayerUI(player);
        }
    }

    private void AddPlayerUI(Photon.Realtime.Player player)
    {
        // PlayerUI prefab'ýný Resources klasöründen yükle
        GameObject playerUIInstance = Resources.Load<GameObject>("PlayerUI");

        // Prefab yüklendi mi kontrol et
        if (playerUIInstance == null)
        {
            Debug.LogError("playerUI prefab could not be loaded from Resources.");
            return;
        }

        // gamePanel atandý mý kontrol et
        if (gamePanel == null)
        {
            Debug.LogError("gamePanel is not assigned.");
            return;
        }

        // Yeni PlayerUI prefab'ýný gamePanel altýnda instantiate et
        GameObject playerUI = Instantiate(playerUIInstance, gamePanel);

        RectTransform rectTransform = playerUI.GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector2(playerCount * 200, 0);

        rectTransform.sizeDelta = new Vector2(200, 50); // Boyutlarý ayarlayabilirsiniz
        rectTransform.pivot = new Vector2(0.5f, 0.5f); // Pivot noktasý ortada

        TMP_Text playerIDText = playerUI.transform.Find("PlayerIDText").GetComponent<TMP_Text>();
        TMP_Text coinText = playerUI.transform.Find("PlayerIDText/CoinText").GetComponent<TMP_Text>();

        // playerIDText veya coinText null mý kontrol et
        if (playerIDText == null)
        {
            Debug.LogError("PlayerIDText component could not be found in the playerUI prefab.");
            return;
        }
        if (coinText == null)
        {
            Debug.LogError("CoinText component could not be found in the playerUI prefab.");
            return;
        }

        playerIDText.text = "Player " + player.ActorNumber; // Oyuncunun ID'si veya ismi

        coinText.text = "0";

        playerCount++;
    }
}
