using UnityEditor.Animations;
using UnityEngine;
using Assets.Scripts.Creatures;
using Assets.Scripts.Collectables;
using Assets.Scripts.Utils;
using Assets.Scripts.Model;
using Assets.Scripts.Component.Health;
using Assets.Scripts.Component.ColliderBased;

namespace Assets.Scripts.Creatures.Hero
{
    public class Hero : Creature
    {
        [Header("Hero")]
        [SerializeField] private float _interactionRadius;
        [SerializeField] private float _fallVelocity;

        [SerializeField] private LayerMask _interactionLayer;

        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        private bool _allowDoubleJump;
        private bool _isSpawnSlamDownDust;

        private GameSession _gameSession;

        #region Unity Functions
        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();

            var health = GetComponent<HealthComponent>();
            health.SetHealth(_gameSession.Data.Hp);
            UpdateHeroWeapon();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (_isSpawnSlamDownDust)
            {
                _isSpawnSlamDownDust = false;
                _particles.Spawn("SlamDown");
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.IsInLayer(_groundLayer))
            {
                var contacts = collision.contacts[0];
                if (contacts.relativeVelocity.y > _fallVelocity)
                {
                    _isSpawnSlamDownDust = true;
                }
            }
        }
        #endregion

        private void UpdateHeroWeapon()
        {
            _animator.runtimeAnimatorController = _gameSession.Data.IsArmed ? _armed : _unarmed;
        }

        protected override float CalculateYVelocity()
        {
            if (_isGrounded) _allowDoubleJump = true;

            return base.CalculateYVelocity();
        }
        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (_allowDoubleJump && !_isGrounded)
            {
                _isSpawnJumpDust = true;
                _allowDoubleJump = false;
                return _jumpSpeed;
            }
            else
            {
                return base.CalculateJumpVelocity(yVelocity);
            }
        }
        public override void Attack()
        {
            if (!_gameSession.Data.IsArmed) return;

            base.Attack();
        }

        public void Collect(GameObject collectableObject)
        {
            if (collectableObject == null) return;

            var collectable = collectableObject.GetComponent<ICollectable>();
            if (collectable != null)
            {
                if (collectable is ScoreCollectable)
                {
                    _gameSession.Data.Coins += (collectable as ScoreCollectable).Score;
                }
            }

            Debug.Log($"Score: {_gameSession.Data.Coins}");
        }
        public void Interact()
        {
            _interactionCheck.Check();
        }
        public void ArmHero()
        {
            _gameSession.Data.IsArmed = true;
            UpdateHeroWeapon();
        }
        public void OnHealthChanged(int currentHealth)
        {
            _gameSession.Data.Hp = currentHealth;
        }
    }
}