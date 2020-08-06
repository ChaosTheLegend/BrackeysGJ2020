using System;
using UnityEngine;

namespace BrackeysGJ.MonoBehaviours
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private MixerManager mixer;
        [SerializeField] private GameObject pausePanel;
        private bool _paused;
        // Update is called once per frame
        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                _paused = !_paused;
                mixer.TogglePause(_paused);
            }
            
            pausePanel.SetActive(_paused);
            Time.timeScale = _paused ? 0f : 1f;
        }

        public void OnClickingResumeButton()
        {
            _paused = !_paused;
            mixer.TogglePause(_paused);
            
            pausePanel.SetActive(_paused);
            Time.timeScale = _paused ? 0f : 1f;
        }

        public void OnClickingExitButton()
        {
            FindObjectOfType<LevelLoader>().LoadNextScene();
        }
    }
}
