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
        [SerializeField] private bool pause;
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
            if(Input.GetKeyDown(KeyCode.Escape)) TogglePause();
        }

        public void SetVolume(SoundType type, float volume)
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
        
        public void TogglePause()
        {
            pause = !pause;
            UpdateState();
        }

        private void UpdateState()
        {
            var trans = pause ? paused : unpaused;
            trans.TransitionTo(0.01f);
        }
        
    }
}
