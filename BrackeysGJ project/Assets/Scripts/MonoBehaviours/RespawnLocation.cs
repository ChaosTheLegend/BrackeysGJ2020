using BrackeysGJ.ClassFiles;
using UnityEngine;

namespace BrackeysGJ.MonoBehaviours
{
    public class RespawnLocation : MonoBehaviour
    {
        [SerializeField] private Transform cam; 
        private void Start()
        {
            var save = SaveSystem.Load();
            transform.position = save.GetPlayerPosition();
            cam.position = save.GetCameraPosition();
        }
    }
}
