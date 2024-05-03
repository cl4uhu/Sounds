using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string volumeParameter;
    private Slider volumeSlider;
    private Toggle muteToggle;
    private bool muted; 

    void Awake()
    {
        volumeSlider = GetComponent<Slider>();
        muteToggle = GetComponentInChildren<Toggle>();

        volumeSlider.onValueChanged.AddListener(ChangeVolume);
        muteToggle.onValueChanged.AddListener(Mute);
    }

    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat(volumeParameter, volumeSlider.maxValue); 

        string muteValue = PlayerPrefs.GetString(volumeParameter + "Mute", "False");

        if(muteValue == "False")
        {
            muted = false; 
        }
        else if(muteValue == "True")
        {
            muted = true;
        }


        muteToggle.isOn = !muted;
    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, volumeSlider.value); 
        PlayerPrefs.SetString(volumeParameter + "Mute", muted.ToString());
    }

    void ChangeVolume(float value)
    {
        mixer.SetFloat(volumeParameter, Mathf.Log10(value) * 20);
    }

    void Mute(bool soundEnabled)
    {
        if(soundEnabled)
        {
            float lastVolume = PlayerPrefs.GetFloat(volumeParameter, volumeSlider.maxValue);
            mixer.SetFloat(volumeParameter, Mathf.Log10(lastVolume) * 20); 
            muted = false;
        }
        else
        {
            PlayerPrefs.SetFloat(volumeParameter, volumeSlider.value);
            mixer.SetFloat(volumeParameter, Mathf.Log10(volumeSlider.minValue) * 20);
            muted = true;
        }
    }
}
