using UnityEditor;
using UnityEngine;

namespace ChaosUtils.Scripts.Editor
{
    [CustomEditor(typeof(AdvPlayerCollider))]
    public class AdvPlayer : UnityEditor.Editor
    {
        private AdvPlayerCollider _playerCollider;
        
        private void OnEnable()
        {
            _playerCollider = (AdvPlayerCollider) target;
        }



        private void OnSceneGUI()
        {
            
            var transform = _playerCollider.transform;
            var position = transform.position;
            var pointSize = _playerCollider.pointSize;
            var pointExtents = _playerCollider.pointExtents;
            var size = Mathf.Min(HandleUtility.GetHandleSize(position) * 0.2f, transform.localScale.magnitude*0.05f);
            
            string[] sides = {"Right","Up","Left","Down"};

            
            Handles.color = Color.green;
            for (var i = 0; i < 4; i++)
            {
                if(_playerCollider.castPoints.Length == 0) break;
                if (_playerCollider.showCollision)
                    Handles.color = _playerCollider.CheckCollision((AdvPlayerCollider.Side) i)
                        ? Color.red
                        : Color.green;
                Handles.DrawLine(position + _playerCollider.castPoints[i*2],position + _playerCollider.castPoints[i*2+1]);   
            
                
                if(!_playerCollider.showLabels) continue;
                Handles.Label(position+pointExtents[i],sides[i]);
            }
            
            if(!_playerCollider.edit) return;
            Handles.color = Color.green;
            for (var i = 0; i < 4; i++)
            {
                pointExtents[i] = Handles.FreeMoveHandle(position+pointExtents[i],Quaternion.identity, size,Vector3.one, Handles.CubeHandleCap) - position;
                if (i % 2 == 0) pointExtents[i].y = 0;
                if (i % 2 == 1) pointExtents[i].x = 0;
            }

            Handles.color = Color.yellow;
            for (var i = 0; i < 8; i++)
            {
                pointSize[i] = Handles.FreeMoveHandle(position + pointExtents[i / 2] + pointSize[i],
                    Quaternion.identity, size, Vector3.one, Handles.CubeHandleCap) - position - pointExtents[i/2];
                if ((i / 2) % 2 == 0) pointSize[i].x = 0f;
                if ((i / 2) % 2 == 1) pointSize[i].y = 0f;

                _playerCollider.castPoints[i] =
                    pointExtents[i / 2] + pointSize[i];
            }
        }
    }
}
