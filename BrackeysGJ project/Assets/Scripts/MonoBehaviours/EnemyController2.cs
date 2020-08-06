using System;
using UnityEngine;

namespace BrackeysGJ.MonoBehaviours
{
    public class EnemyController2 : MonoBehaviour
    {
        [SerializeField] private float detectionRadius;
        [SerializeField] private float speed;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private float shootDelay;
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform shootingPoint;
        [SerializeField] private Transform shotFlipper;
        [SerializeField] private HealthBar healthBar;
        public float health = 100f;
        public Transform sticker;
        private SpriteRenderer _sprite;
        private PathFolower _follower;
        private Transform _player;
        private PlayerHealth _playerHealth;
        private Rigidbody2D _rb;
        [HideInInspector]public bool canSee;
        private float _tm;

        private EnemySound _sounds;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            _playerHealth = _player.GetComponent<PlayerHealth>();
            healthBar.SetMaxHealth(health);
            _rb = GetComponent<Rigidbody2D>();
            _sprite = GetComponent<SpriteRenderer>();
            _follower = GetComponent<PathFolower>();
            _follower.speed = speed;
            _follower.StartFollow();

            _sounds = GetComponent<EnemySound>();

            if(_sounds == null)
            {
                Debug.LogError("EnemySound.cs is missing on the " + gameObject.name + " gameObject!");
            }
        }
        
        private void CheckEyesight()
        {
            if (_playerHealth.CheckIfDead())
            {
                canSee = false;
                return;
            }
            var cast = Physics2D.Linecast(transform.position, _player.position, wallLayer);
            if (cast) return;
            canSee = true;
            _follower.speed = 0f;
            _follower.UpdateSpeed();
        }

        
        public void TakeDamage(float damage)
        {
            health -= damage;
            healthBar.SetHealth(health);
            if (!(health <= 0))
            {
                _sounds.PlayHitSound();
            }
            else
            {
                _sounds.PlayDeathSound();
                for (var i = 0; i < sticker.childCount; i++)
                {
                    sticker.GetChild(i).GetComponent<Shuriken>().Rewind();
                }
            }
        }
        
        private void Shoot()
        {
            Instantiate(bullet, shootingPoint.position, shootingPoint.rotation);
            _sounds.PlayShootSound();
        }
        private void Update()
        {
            var dis = transform.position - _player.position;
            if (dis.sqrMagnitude < detectionRadius * detectionRadius)
                CheckEyesight();
            else canSee = false;

            if(!canSee){
                _sounds.PlayRunSoundLoop();
                _follower.speed = speed;
                _follower.UpdateSpeed();
            }
            else
            {
                _sounds.StopRunSoundLoop();
            }
            
            if (health <= 0)
            {
                _follower.speed = 0f;
                _follower.UpdateSpeed();
                _rb.simulated = false;
                canSee = false;
                return;
            }
            
            _sprite.flipX = _follower.GetDir() > 0f;
            if (canSee)
            {
                if (_tm <= 0f)
                {
                    Shoot();
                    _tm = shootDelay;
                }

                _sprite.flipX = dis.x > 0f;
            }
            
            shotFlipper.localScale = new Vector3(_sprite.flipX?1f:-1f,1f,1f);

            if (_tm > 0)
            {
                _tm -= Time.deltaTime;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.67f, 0.04f);
            Gizmos.DrawWireSphere(transform.position,detectionRadius);

            if (_player == null) return;
            if ((transform.position - _player.position).sqrMagnitude > detectionRadius * detectionRadius) return;
            Gizmos.color = canSee?Color.red : Color.green;;
            Gizmos.DrawLine(transform.position,_player.position);
        }
    }
}
