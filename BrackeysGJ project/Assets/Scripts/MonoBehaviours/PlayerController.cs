using ChaosUtils.Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace BrackeysGJ.MonoBehaviours
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        [Header("Speed")]
        [SerializeField] private float speed = 1f;
        [SerializeField] private float climbingSpeed = 1f;
        [Header("Terminal velocity")] 
        [SerializeField] private float maxFallingSpeed = 1f;
        [SerializeField] private float maxSlidingSpeed = 1f;
        [Header("Gravity change")]
        [SerializeField] private float defaultGravity = 1f;
        [SerializeField] private float wallGravity = 1f;
        [Header("Jumps")]
        [SerializeField] private float jumpForce = 1f;
        [SerializeField] private float wallJumpForce = 1f;
        [SerializeField] private float doubleJumpForce = 1f;
        [SerializeField] private float wallJumpRebound = 1f;
        [SerializeField] private float reboundTime = 1f;
        [Header("Dash")] 
        [SerializeField] private float dashTime = 1f;
        [SerializeField] private float dashForce = 1f;

        [Header("Events")] 
        public UnityEvent onJump;
        public UnityEvent onWallJump;
        public UnityEvent onDoubleJump;
        public UnityEvent onDash;
        
        private float _reboundTm;
        private float _reboundDir;
        private float _dashDir;
        private float _dashTm;
        private AdvPlayerCollider _advCol;
        private bool _canDoubleJump;
        private bool _canDash;
        private bool _onLadder;
        private void Start()
        {
            _advCol = GetComponent<AdvPlayerCollider>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag($"Ladder")) return;
            _onLadder = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag($"Ladder")) return;
            _onLadder = false;
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
            
            //Wall rebound;
            if (_reboundTm > 0 && !_onLadder)
            {
                _reboundTm -= Time.deltaTime;
                _rb.velocity = new Vector2(_reboundDir * speed * wallJumpRebound, _rb.velocity.y);
            }
            
            
            //Dashing
            if (_dashTm > 0)
            {
                _dashTm -= Time.deltaTime;
                _rb.velocity = new Vector2(_dashDir * dashForce, _rb.velocity.y);
            }
            
            if (Input.GetButtonDown($"Dash") && _canDash)
            {
                onDash?.Invoke();
                _dashTm = dashTime;
                _dashDir = Mathf.Sign(Input.GetAxis("Horizontal"));
                _canDash = false;
            }

            
            //Wall sliding
            _rb.gravityScale = OnWall() ? wallGravity : defaultGravity;
            
            //Ladder climbing
            if (_onLadder)
            {
                _reboundTm = 0f;
                _rb.gravityScale = 0f;
                _rb.velocity = new Vector2(_rb.velocity.x, Input.GetAxis("Vertical")*climbingSpeed);
                _canDoubleJump = true;
                return;
            }

            //Terminal velocity check
            if(_rb.velocity.y < maxFallingSpeed*-1f) _rb.velocity = new Vector2(_rb.velocity.x, maxFallingSpeed*-1f);
            
            if (OnWall())
            {
                _canDoubleJump = false;
                _canDash = true;
                if(_rb.velocity.y < maxSlidingSpeed*-1f) _rb.velocity = new Vector2(_rb.velocity.x, maxSlidingSpeed*-1f);
            }

            if (_advCol.IsGrounded())
            {
                _rb.gravityScale = 0f;
                _canDoubleJump = true;
                _canDash = true;
            }
            
            //Jumps
            if (!Input.GetButtonDown("Jump")) return;
            
            //Ground Jump
            if (_advCol.IsGrounded())
            {
                onJump?.Invoke();
                _rb.AddForce(Vector2.up * jumpForce);
            }
            else if (OnWall())
            {
                //Wall Jump
                onWallJump?.Invoke();
                _reboundDir = _advCol.CheckCollision(AdvPlayerCollider.Side.Right) ? -1f : 1f;
                _rb.velocity = new Vector2(_rb.velocity.x,0f);
                _rb.AddForce(Vector2.up*wallJumpForce);
                _reboundTm = reboundTime;
            }
            else if (_canDoubleJump)
            {
                //Double Jump
                onDoubleJump?.Invoke();
                _rb.velocity = new Vector2(_rb.velocity.x,0f);
                _rb.AddForce(Vector2.up*doubleJumpForce);
                _canDoubleJump = false;
            }
        }
    }
}
