
using UnityEngine;

namespace BrackeysGJ.Serializable
{
    [System.Serializable]
    public class SaveData
    {
        private float[] _position;
        private float[] _camPosition;
        public SaveData(Vector2 playerPos, Vector2 camPos)
        {
            _position = new[]{playerPos.x, playerPos.y};
            _camPosition = new[]{camPos.x, camPos.y};
        }

        public Vector2 GetPlayerPosition()
        {
            return new Vector2(_position[0],_position[1]);
        }

        public Vector3 GetCameraPosition()
        {
            return new Vector3(_camPosition[0],_camPosition[1],-100f);
        }
    }
}
