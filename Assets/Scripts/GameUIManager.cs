using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameUIManager : MonoBehaviourPunCallbacks
{
    public Transform gamePanel; // gamePanel objesini burada tan�ml�yoruz
    private int playerCount = 0;

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        AddPlayerUI(newPlayer);
    }

    private void AddPlayerUI(Photon.Realtime.Player player)
    {
        // PlayerText prefab'�n� Resources klas�r�nden y�kle
        GameObject playerInfoPrefab = Resources.Load<GameObject>("Assets/Resources/PlayerIDText.prefab");

        if (playerInfoPrefab != null)
        {
            // Yeni PlayerText prefab'�n� gamePanel alt�nda instantiate et
            GameObject playerInfoInstance = Instantiate(playerInfoPrefab, gamePanel);

            // Pozisyonunu ayarla
            RectTransform rectTransform = playerInfoInstance.GetComponent<RectTransform>();

            // anchoredPosition kullanarak pozisyonu ayarl�yoruz
            rectTransform.anchoredPosition = new Vector2(playerCount * 200, 0);

            // Boyutlar�n� ve pivot ayarlar�n� kontrol edelim
            rectTransform.sizeDelta = new Vector2(200, 50); // Boyutlar� ayarlayabilirsiniz
            rectTransform.pivot = new Vector2(0.5f, 0.5f); // Pivot noktas� ortada

            // Player ID ve Coin Text'lerini ayarla
            TMP_Text playerIDText = playerInfoInstance.transform.Find("PlayerIDText").GetComponent<TMP_Text>();
            TMP_Text coinText = playerInfoInstance.transform.Find("CoinText").GetComponent<TMP_Text>();
            playerIDText.text = "Player " + player.ActorNumber; // Oyuncunun ID'si veya ismi

            // Gerekirse coinText ayarlar�n� da burada yapabilirsiniz
            coinText.text = "0";

            playerCount++;
        }
        else
        {
            Debug.LogError("PlayerText prefab could not be loaded from Resources.");
        }
    }
}
