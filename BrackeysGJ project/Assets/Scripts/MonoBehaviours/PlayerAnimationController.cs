using System;
using ChaosUtils.Scripts;
using UnityEngine;

namespace BrackeysGJ.MonoBehaviours
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private AdvPlayerCollider _advCol;
        private PlayerController _playerController;
        private ShurikenThrow _throw;
        private Animator _anim;
        private Rigidbody2D _rb;
        private SpriteRenderer _renderer;
        private bool _dir;
        private bool _shooting;
        private void Start()
        {
            _throw = GetComponent<ShurikenThrow>();
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
            _shooting = true;
            Invoke($"ResetShot",0.3f);
        }

        private void ResetDoubleJump() => _anim.SetBool($"DoubleJump",false);
        private void ResetShot() => _shooting = false;
        
        private void Update()
        {
            var move = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5f;

            _dir = _rb.velocity.x < 0f;
            if(!move || _shooting) _dir = !_throw.dir;
            if (_playerController.OnWall()) _dir = _advCol.CheckCollision(AdvPlayerCollider.Side.Right);
            _renderer.flipX = _dir;

            _anim.SetBool($"Shooting",_shooting);
            _anim.SetBool($"Grounded",_advCol.IsGrounded());
            _anim.SetBool($"OnWall",_playerController.OnWall());
            _anim.SetBool($"Moving",move);
            _anim.SetBool($"OnLadder",_playerController.OnLadder());
            var climb = Math.Abs(Input.GetAxis("Vertical")) > 0.5f;
            climb = _playerController.OnLadder() && climb;
            _anim.SetBool($"Climbing",climb);
        }
    }
}
