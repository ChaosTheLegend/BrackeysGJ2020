using System.Collections;
using System.Collections.Generic;
using BrackeysGJ.MonoBehaviours;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    private float defaultMusicVolume = 0f;

    [SerializeField] private Slider soundEffectsVolumeSlider;
    private float defaultSoundEffectsVolume = 0f;
    
    private void Start()
    {
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume",defaultMusicVolume);
        }
        
        if (!PlayerPrefs.HasKey("SoundVolume"))
        {
            PlayerPrefs.SetFloat("SoundVolume",defaultSoundEffectsVolume);
        }
        
        
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        soundEffectsVolumeSlider.value = PlayerPrefs.GetFloat("SoundVolume");
    }

    // Update is called once per frame

    public void OnMusicChange()
    {
        PlayerPrefs.SetFloat("MusicVolume",musicVolumeSlider.value);
    }
    
    public void OnSfxChange()
    {
        PlayerPrefs.SetFloat("SoundVolume",soundEffectsVolumeSlider.value);
    }
    
    public void SaveAndExit()
    {
        PlayerPrefs.Save();
        FindObjectOfType<LevelLoader>().LoadNextScene();
    }

    public void SetDefault()
    {
        musicVolumeSlider.value = defaultMusicVolume;
        soundEffectsVolumeSlider.value = defaultSoundEffectsVolume;
    }
}
