using System;
using UnityEngine;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace BrackeysGJ.MonoBehaviours
{
    public class MixerManager : MonoBehaviour
    {
        [Header("Pause")]
        [SerializeField] private AudioMixerSnapshot paused;
        [SerializeField] private AudioMixerSnapshot unpaused;
        private bool pause;
        [Header("Volume")] 
        [SerializeField] private AudioMixer mixer;

        public enum SoundType
        {
            Music=0,
            Sfx=1,
            Ambient=2
        }


        private void Update()
        {
            mixer.GetFloat("MusicVolume", out var muse);
            mixer.GetFloat("SFXVolume", out var sfx);
            if (Math.Abs(muse - PlayerPrefs.GetFloat("MusicVolume")) > 0.01f) SetVolume(SoundType.Music,PlayerPrefs.GetFloat("MusicVolume"));
            if (Math.Abs(sfx - PlayerPrefs.GetFloat("SoundVolume")) > 0.01f) SetVolume(SoundType.Sfx,PlayerPrefs.GetFloat("SoundVolume"));
        }

        private void SetVolume(SoundType type, float volume)
        {
            switch (type)
            {
                case SoundType.Music:
                    mixer.SetFloat("MusicVolume", volume);
                    break;
                case SoundType.Sfx:
                    mixer.SetFloat("SFXVolume", volume);
                    break;
                case SoundType.Ambient:
                    mixer.SetFloat("AmbientVolume", volume);
                    break;
                default:
                    break;
            }
        }
        
        public void TogglePause(bool toggle)
        {
            pause = toggle;
            UpdateState();
        }

        private void UpdateState()
        {
            var trans = pause ? paused : unpaused;
            trans.TransitionTo(0.2f);
        }
        
    }
}
