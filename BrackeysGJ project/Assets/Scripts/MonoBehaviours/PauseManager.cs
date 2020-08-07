using System;
using UnityEngine;

namespace BrackeysGJ.MonoBehaviours
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private MixerManager mixer;
        [SerializeField] private GameObject pausePanel;
        public static bool Paused;
        // Update is called once per frame
        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                Paused = !Paused;
                mixer.TogglePause(Paused);
            }
            
            pausePanel.SetActive(Paused);
            Time.timeScale = Paused ? 0f : 1f;
        }

        public void OnClickingResumeButton()
        {
            Paused = !Paused;
            mixer.TogglePause(Paused);
            
            pausePanel.SetActive(Paused);
            Time.timeScale = Paused ? 0f : 1f;
        }

        public void OnClickingExitButton()
        {
            FindObjectOfType<LevelLoader>().LoadNextScene();
        }
    }
}
