using System;
using ChaosUtils.Scripts;
using UnityEngine;

namespace BrackeysGJ.MonoBehaviours
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private AdvPlayerCollider _advCol;
        private PlayerController _playerController;
        private Animator _anim;
        private Rigidbody2D _rb;
        private SpriteRenderer _renderer;
        private bool _dir;
        private void Start()
        {
            _advCol = GetComponent<AdvPlayerCollider>();
            _playerController = GetComponent<PlayerController>();
            _anim = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
        }


        public void DoubleJump()
        {
            _anim.SetBool($"DoubleJump",true);
            Invoke($"ResetDoubleJump",0.1f);
        }

        public void Shoot()
        {
            _anim.SetBool($"Shoot",true);
            Invoke($"ResetShot",0.1f);
        }

        private void ResetDoubleJump() => _anim.SetBool($"DoubleJump",false);
        private void ResetShot() => _anim.SetBool($"Shoot",false);
        
        private void Update()
        {
            _dir = _rb.velocity.x < 0f;
            if (_playerController.OnWall()) _dir = _advCol.CheckCollision(AdvPlayerCollider.Side.Right);
            _renderer.flipX = _dir;

            
            _anim.SetBool($"Grounded",_advCol.IsGrounded());
            _anim.SetBool($"OnWall",_playerController.OnWall());
            var move = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5f;
            _anim.SetBool($"Moving",move);
            _anim.SetBool($"OnLadder",_playerController.OnLadder());
            var climb = Math.Abs(Input.GetAxis("Vertical")) > 0.5f;
            climb = _playerController.OnLadder() && climb;
            _anim.SetBool($"Climbing",climb);
        }
    }
}
