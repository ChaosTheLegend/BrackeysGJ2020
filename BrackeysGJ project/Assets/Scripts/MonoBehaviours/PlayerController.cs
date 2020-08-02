using ChaosUtils.Scripts;
using UnityEngine;

namespace BrackeysGJ.MonoBehaviours
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        [SerializeField] private float speed = 1f;
        [SerializeField] private float defaultGravity = 1f;
        [SerializeField] private float wallGravity = 1f;
        [SerializeField] private float jumpForce = 1f;
        [SerializeField] private float wallJumpForce = 1f;
        [SerializeField] private float wallJumpRebound = 1f;
        [SerializeField] private float reboundTime = 1f;

        private float _reboundTm;
        private float _reboundDir;
        private AdvPlayerCollider _advCol;
        private void Start()
        {
            _advCol = GetComponent<AdvPlayerCollider>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private bool OnWall()
        {
            return (_advCol.CheckCollision(AdvPlayerCollider.Side.Left) || _advCol.CheckCollision(AdvPlayerCollider.Side.Right));
        }
        private void Update()
        {
            //Movement
            _rb.velocity = new Vector2(Input.GetAxis("Horizontal")*speed,_rb.velocity.y);
            if (_advCol.CheckCollision(AdvPlayerCollider.Side.Left))
                _rb.velocity = new Vector2(Mathf.Max(0f, _rb.velocity.x), _rb.velocity.y);
            if (_advCol.CheckCollision(AdvPlayerCollider.Side.Right))
                _rb.velocity = new Vector2(Mathf.Min(0f, _rb.velocity.x), _rb.velocity.y);

            if (_reboundTm > 0)
            {
                _reboundTm -= Time.deltaTime;
                _rb.velocity = new Vector2(_reboundDir * speed * wallJumpRebound, _rb.velocity.y);
            }
            
            
            //Sliding
            _rb.gravityScale = OnWall() ? wallGravity : defaultGravity;

            //Jumps
            if (!Input.GetButtonDown("Jump")) return;
            
            //Ground Jump
            if (_advCol.IsGrounded()) _rb.AddForce(Vector2.up * jumpForce);
            else if (OnWall())
            {
                //Wall Jump
                _reboundDir = _advCol.CheckCollision(AdvPlayerCollider.Side.Right) ? -1f : 1f;
                _rb.velocity = new Vector2(_rb.velocity.x,0f);
                _rb.AddForce(Vector2.up*wallJumpForce);
                _reboundTm = reboundTime;
            }
        }
    }
}
