using System;
using UnityEngine;

namespace BrackeysGJ.MonoBehaviours
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private MixerManager mixer;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject healthbar;
        [SerializeField] private GameObject shurikenGui;
        [SerializeField] private GameObject label;
        [SerializeField] private FadeManager fader;
        public static bool Paused;
        // Update is called once per frame
        private void Start()
        {
            fader.FadeOut();
        }

        private void Update()
        {
            if (DoggoController.win)
            {
                healthbar.SetActive(false);
                shurikenGui.SetActive(false);
                pausePanel.SetActive(false);
                label.SetActive(false);
                return;
            }
            
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
            Paused = !Paused;
            Time.timeScale = 1f;
            mixer.TogglePause(false);
            FindObjectOfType<LevelLoader>().LoadNextScene();
        }
    }
}
