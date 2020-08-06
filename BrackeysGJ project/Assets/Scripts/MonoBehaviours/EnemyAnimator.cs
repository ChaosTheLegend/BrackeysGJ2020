using System;
using BrackeysGJ.MonoBehaviours;
using UnityEngine;

namespace BrackeysGJ
{
    public class EnemyAnimator : MonoBehaviour
    {
        private Animator _anim;
        private EnemyController2 _controller;
        private void Start()
        {
            _anim = GetComponent<Animator>();
            _controller = GetComponent<EnemyController2>();
        }
        
        private void Update()
        {
            _anim.SetBool($"Shooting",_controller.canSee);
            if (_controller.health <= 0) _anim.SetTrigger($"Death");
            
        }
    }
}
