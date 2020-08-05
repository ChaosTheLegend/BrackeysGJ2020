using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsController : MonoBehaviour
{
    const string MUSIC_VOLUME_KEY = "music volume";
    const string SOUNDEFFECTS_VOLUME_KEY = "sound effects volume";

    const float MIN_VOLUME = 0f;
    const float MAX_VOLUME = 1f;

    public static void SetSoundEffectsVolume(float volume)
    {
        if (volume >= MIN_VOLUME && volume <= MAX_VOLUME)
            PlayerPrefs.SetFloat(SOUNDEFFECTS_VOLUME_KEY, volume);
    }

    public static void SetMusicVolume(float volume)
    {
        if (volume >= MIN_VOLUME && volume <= MAX_VOLUME)
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
    }

    public static float GetSoundEffectsVolume()
    {
        return PlayerPrefs.GetFloat(SOUNDEFFECTS_VOLUME_KEY);
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
    }
}
