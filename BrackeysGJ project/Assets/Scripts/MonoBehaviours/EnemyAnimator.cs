using System;
using BrackeysGJ.MonoBehaviours;
using UnityEngine;

namespace BrackeysGJ
{
    public class EnemyAnimator : MonoBehaviour
    {
        private Animator _anim;
        private EnemyController2 _controller;
        [SerializeField] private ParticleSystem deathParticle;
        [SerializeField] private GameObject deathLight;
        [SerializeField] private GameObject canvas;
        private SpriteRenderer _spr;
        private bool _dead;
        private void Start()
        {
            _spr = GetComponent<SpriteRenderer>();
            _anim = GetComponent<Animator>();
            _controller = GetComponent<EnemyController2>();
        }
        
        private void Update()
        {
            _anim.SetBool($"Shooting",_controller.canSee);
            if (_controller.Health <= 0 && !_dead)
            {
                deathParticle.Play();
                deathLight.SetActive(true);
                _dead = true;
                _anim.SetTrigger($"Death");
                _spr.enabled = false;
                canvas.SetActive(false);
                Destroy(gameObject,1f);
            }
        }
    }
}
