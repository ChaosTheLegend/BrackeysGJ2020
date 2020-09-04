using ChaosUtils.Scripts;
using System;
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
        [Header("Slopes")] 
        [SerializeField] private float slopeThreshold;
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
        private bool _nearLadder;
        private bool _dead;

        private PlayerSound _sounds;

        internal enum PlayerState
        {
            NoneOfTheAbove,
            Running,
            Sliding,
            Climbing
        }

        // These are used for sound (I know it's messy but I'm trying to rush this and make it work)
        internal PlayerState myPreviousState = PlayerState.Running;
        internal PlayerState myCurrentState = PlayerState.Running;

        private void Start()
        {
            _advCol = GetComponent<AdvPlayerCollider>();
            _rb = GetComponent<Rigidbody2D>();
            _sounds = GetComponentInChildren<PlayerSound>();

            if(_sounds == null)
            {
                Debug.LogError("Error: PlayerSound.cs is missing on the " + gameObject.name + " gameObject.");
            }
        }

        public bool OnLadder()
        {
            return _onLadder;
        }

        public void Die()
        {
            _dead = true;
            _rb.velocity = Vector2.zero;
        }

        public bool IsDead()
        {
            return _dead;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag($"Ladder")) return;
            _nearLadder = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag($"Ladder")) return;
            _onLadder = false;
            _nearLadder = false;
        }

        public bool CollisionLeft()
        {
            var left = _advCol.CheckCollision(AdvPlayerCollider.Side.Left);

            if (left)
            {
                float angle = Mathf.Atan2(left.normal.y, left.normal.x) * Mathf.Rad2Deg - 90f;
                if (!(angle < 45f && angle > -45f))
                {
                    return true;
                }
            }

            return false;
        }
            
        public bool CollisionRight()
        {
            var right = _advCol.CheckCollision(AdvPlayerCollider.Side.Right);
            if (right)
            {
                float angle = Mathf.Atan2(right.normal.y, right.normal.x) * Mathf.Rad2Deg - 90f;
                if (!(angle < 45f && angle > -45f))
                {
                    return true;
                }
            }

            return false;
        }
        
        public bool OnWall()
        {
            return (CollisionLeft() || CollisionRight());
        }
        
        
        //The time has come
        //to do what must be done
        //I have to fix the slopes
        //Or die trying...
        //-ChaosTheLeg 4AM

        /// <summary>
        /// Casts a line from player position to the point of collision on surface
        /// this is done to correctly calculate angle of the normal
        /// </summary>
        private RaycastHit2D DirectCast(RaycastHit2D surface)
        {
            return Physics2D.Linecast(transform.position, surface.point,_advCol.getLayer());
        }
        
        /// <summary>
        /// Checks if the surface is supposed to be considered as "ground" or not 
        /// </summary>
        private bool IsGround(RaycastHit2D surface)
        {
            RaycastHit2D trueSurface = DirectCast(surface);
            Vector2 norm = trueSurface.normal;
            float angle = Mathf.Atan2(norm.y, norm.x) * Mathf.Rad2Deg - 90;
            return angle > -45 && angle < 45;
        }
        
        
        private void Update()
        {
            
            if(PauseManager.Paused) return;
            if (DoggoController.win) return;

            myPreviousState = myCurrentState;
            SoundLoopLogic();
            
            if (_dead) return;
            


            //Movement
            var ground = _advCol.CheckCollision(AdvPlayerCollider.Side.Down);
            if (IsGround(ground))
            {
                var norm = DirectCast(ground).normal;
                var dir = (Quaternion.Euler(0f, 0f, -90f) * norm).normalized;
                _rb.velocity = Input.GetAxis("Horizontal") * speed * dir;

            }
            else
            {
                _rb.velocity = new Vector2(Input.GetAxis("Horizontal")*speed,_rb.velocity.y);
            }

            if (CollisionLeft())
                _rb.velocity = new Vector2(Mathf.Max(0f, _rb.velocity.x), _rb.velocity.y);
            if (CollisionRight())
                _rb.velocity = new Vector2(Mathf.Min(0f, _rb.velocity.x), _rb.velocity.y);
            
            //Wall rebound;
            if (_reboundTm > 0 && !_onLadder)
            {
                _reboundTm -= Time.deltaTime;
                _rb.velocity = new Vector2(_reboundDir * speed * wallJumpRebound, _rb.velocity.y);
            }

            if (_nearLadder && Mathf.Abs(Input.GetAxis("Vertical")) > 0f) _onLadder = true;
            
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
            if (Input.GetButtonDown("Jump"))
            {
                myCurrentState = PlayerState.NoneOfTheAbove;

                //Ground Jump
                if (_advCol.IsGrounded())
                {
                    onJump?.Invoke();
                    _rb.AddForce(Vector2.up * jumpForce);
                    _sounds.PlayJumping();
                }
                else if (OnWall())
                {
                    //Wall Jump
                    onWallJump?.Invoke();
                    _reboundDir = _advCol.CheckCollision(AdvPlayerCollider.Side.Right) ? -1f : 1f;
                    _rb.velocity = new Vector2(_rb.velocity.x, 0f);
                    _rb.AddForce(Vector2.up * wallJumpForce);
                    _reboundTm = reboundTime;

                    _sounds.PlayJumping();
                    _sounds.StopSounds();
                }
                else if (_canDoubleJump)
                {
                    //Double Jump
                    onDoubleJump?.Invoke();
                    _rb.velocity = new Vector2(_rb.velocity.x, 0f);
                    _rb.AddForce(Vector2.up * doubleJumpForce);
                    _canDoubleJump = false;

                    _sounds.PlayDoubleJumping();
                }
            }

            if(myPreviousState != myCurrentState)
            {
                _sounds.StopSounds();
            }
        }

        
        /// <summary>
        /// Checks which sounds need to be played in a loop and tells PlayerSounds.cs to play them.
        /// </summary>
        private void SoundLoopLogic()
        {

            if (_onLadder)
            {
                if (_rb.velocity.y != 0)
                {
                    myCurrentState = PlayerState.Climbing;
                    //Debug.Log("State set to climbing.");

                }
                else
                {
                    _sounds.StopSounds();
                    myCurrentState = PlayerState.NoneOfTheAbove;
                }
            }
            else if (OnWall())
            {
                if (_rb.velocity.y < 0)
                {
                    myCurrentState = PlayerState.Sliding;
                    //Debug.Log("State set to sliding.");
                }
            }
            else if (_advCol.IsGrounded())
            {
                if (_rb.velocity.x != 0)
                {
                    myCurrentState = PlayerState.Running;
                }
                else
                {
                    _sounds.StopSounds();
                    myCurrentState = PlayerState.NoneOfTheAbove;
                }
            }
            else
            {
                myCurrentState = PlayerState.NoneOfTheAbove;
            }


                switch (myCurrentState)
            {
                case PlayerState.NoneOfTheAbove:
                    break;
                case PlayerState.Running:
                    _sounds.PlayRunning();
                    break;
                case PlayerState.Sliding:
                    _sounds.PlayWallSlide();
                    break;
                case PlayerState.Climbing:
                    _sounds.PlayClimbing();
                    break;
                default:
                    break;
            }
        }
        
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            Gizmos.color = Color.red;
            var ground1 = _advCol.CheckCollision(AdvPlayerCollider.Side.Down);
            
            if (ground1)
            {
                
                var ground = Physics2D.Linecast(transform.position, ground1.point,_advCol.getLayer());

                Gizmos.DrawSphere(ground.point,0.1f);
                Gizmos.color = IsGround(ground1) ? Color.green : Color.yellow;
                float angle = Mathf.Atan2(ground.normal.y, ground.normal.x) * Mathf.Rad2Deg;
                print(ground.normal+" "+angle);
                
                var dir = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
                Gizmos.DrawLine(transform.position,transform.position + dir);

            }
        }

        // Written by blindspot, edit it how you want chaos;
        // public void TakeDamage(float damage)
        // {
        //     health -= damage;
        //     healthBarPrefab.SetHealth(health);
        //     if (health <= 0)
        //     {
        //         Destroy(gameObject);
        //     }
        // }
    }
}
