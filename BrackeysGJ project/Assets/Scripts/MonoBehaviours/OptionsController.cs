using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    private float defaultMusicVolume = 0.5f;

    [SerializeField] private Slider soundEffectsVolumeSlider;
    private float defaultSoundEffectsVolume = 0.5f;
    
    
    void Start()
    {
        musicVolumeSlider.value = PlayerPrefsController.GetMusicVolume();
        if (musicVolumeSlider.value <= 0) musicVolumeSlider.value = defaultMusicVolume;

        soundEffectsVolumeSlider.value = PlayerPrefsController.GetSoundEffectsVolume();
        if (soundEffectsVolumeSlider.value <= 0) soundEffectsVolumeSlider.value = defaultMusicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        // Run these codes when sripts with music and sound effects are present;
        // Write a function SetVolume in both of them; 
        
        // var musicPlayer = FindObjectOfType<MusicPlayer>();
        // if (musicPlayer)
        // {
        //     musicPlayer.SetVolume(musicVolumeSlider.value);
        // }

        // var soundEffects = FindObjectOfType<SoundEffects>();
        // if (soundEffects)
        // {
        //     soundEffects.SetVolume(soundEffects)
        // }
    }

    public void SaveAndExit()
    {
        PlayerPrefsController.SetMusicVolume(musicVolumeSlider.value);
        FindObjectOfType<LevelLoader>().LoadNextScene();
    }

    public void SetDefault()
    {
        musicVolumeSlider.value = defaultMusicVolume;
        soundEffectsVolumeSlider.value = defaultSoundEffectsVolume;
    }
}
