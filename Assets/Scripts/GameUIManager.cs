using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameUIManager : MonoBehaviourPunCallbacks
{
    public Transform gamePanel; // gamePanel objesini burada tanýmlýyoruz
    private int playerCount = 0;

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        AddPlayerUI(newPlayer);
    }

    private void AddPlayerUI(Photon.Realtime.Player player)
    {
        // PlayerText prefab'ýný Resources klasöründen yükle
        GameObject playerInfoPrefab = Resources.Load<GameObject>("Assets/Resources/PlayerIDText.prefab");

        if (playerInfoPrefab != null)
        {
            // Yeni PlayerText prefab'ýný gamePanel altýnda instantiate et
            GameObject playerInfoInstance = Instantiate(playerInfoPrefab, gamePanel);

            // Pozisyonunu ayarla
            RectTransform rectTransform = playerInfoInstance.GetComponent<RectTransform>();

            // anchoredPosition kullanarak pozisyonu ayarlýyoruz
            rectTransform.anchoredPosition = new Vector2(playerCount * 200, 0);

            // Boyutlarýný ve pivot ayarlarýný kontrol edelim
            rectTransform.sizeDelta = new Vector2(200, 50); // Boyutlarý ayarlayabilirsiniz
            rectTransform.pivot = new Vector2(0.5f, 0.5f); // Pivot noktasý ortada

            // Player ID ve Coin Text'lerini ayarla
            TMP_Text playerIDText = playerInfoInstance.transform.Find("PlayerIDText").GetComponent<TMP_Text>();
            TMP_Text coinText = playerInfoInstance.transform.Find("CoinText").GetComponent<TMP_Text>();
            playerIDText.text = "Player " + player.ActorNumber; // Oyuncunun ID'si veya ismi

            // Gerekirse coinText ayarlarýný da burada yapabilirsiniz
            coinText.text = "0";

            playerCount++;
        }
        else
        {
            Debug.LogError("PlayerText prefab could not be loaded from Resources.");
        }
    }
}
