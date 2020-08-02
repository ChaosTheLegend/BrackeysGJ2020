using UnityEngine;

namespace ChaosUtils.Scripts
{
    public class AdvPlayerCollider : MonoBehaviour
    {
        [HideInInspector]
        public Vector3[] castPoints = new Vector3[8];

        [HideInInspector] public Vector3[] pointExtents = {
            Vector3.right,
            Vector3.up,
            Vector3.left,
            Vector3.down
        };
        [HideInInspector] public Vector3[] pointSize = 
        {
            Vector3.right+Vector3.up*0.5f,
            Vector3.right+Vector3.down*0.5f,
            
            Vector3.up + Vector3.left*0.5f,
            Vector3.up + Vector3.right*0.5f,

            Vector3.left + Vector3.up*0.5f,
            Vector3.left + Vector3.down*0.5f,
            
            Vector3.down + Vector3.right * 0.5f,
            Vector3.down + Vector3.left * 0.5f
        };

        public bool edit = true;
        public bool showLabels;
        public bool showCollision;

        [SerializeField]
        private LayerMask castLayer = 0;
        
        public enum Side
        {
            Up=1,
            Down=3,
            Left=2,
            Right=0
        }

        public RaycastHit2D CheckCollision(Side side)
        {
            var position = transform.position;
            var cast = Physics2D.Linecast(position + castPoints[(int) side * 2], position + castPoints[(int) side * 2 + 1], castLayer);
            return cast;
        }

        public bool IsGrounded()
        {
            return CheckCollision(side: Side.Down);
        }
    }
}
