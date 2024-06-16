using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public static Settings instance;

    public Slider musicSlider;
    public AudioSource music;
    public TextMeshProUGUI valueText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        }
        else
        {
            Destroy(this.gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        musicSlider.value = 0.5f;
        music.volume = musicSlider.value;
        valueText.text = ((int)(musicSlider.value * 100)).ToString();
    }
    public void OnSliderChanged (float volume)
    {
        music.volume = volume;
        int volumeInt = (int)(volume * 100);
        valueText.text = volumeInt.ToString();
    }
}
