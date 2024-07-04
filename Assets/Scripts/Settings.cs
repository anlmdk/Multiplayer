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
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        musicSlider.value = savedVolume;
        music.volume = savedVolume;
        valueText.text = ((int)(savedVolume * 100)).ToString();
    }
    public void OnSliderChanged (float volume)
    {
        music.volume = volume;
        int volumeInt = (int)(volume * 100);
        valueText.text = volumeInt.ToString();

        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}
