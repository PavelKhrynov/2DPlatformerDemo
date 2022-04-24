using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhereIAm.Scripts.Component;
using WhereIAm.Scripts.Utils;

namespace WhereIAm.Scripts
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _damageJumpSpeed;
        [SerializeField] private float _interactionRadius;
        [SerializeField] private float _fallVelocity;

        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private LayerMask _groundLayer;

        [Space] [Header("Particles")]
        [SerializeField] private SpawnComponent _footStepsDust;
        [SerializeField] private SpawnComponent _jumpDust;
        [SerializeField] private SpawnComponent _fallDust;

        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Animator _animator;
        private Collider2D[] _intractionResult = new Collider2D[1];

        private bool _isGrounded;
        private bool _allowDoubleJump;
        private bool _isSpawnJumpDust;
        private bool _isSpawnFallDust;
        private int _score;

        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int IsRunningKey = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int Hit = Animator.StringToHash("hit");

        #region Unity Functions
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _isGrounded = IsGrounded();
        }

        private void FixedUpdate()
        {
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            if (_isSpawnJumpDust)
            {
                _isSpawnJumpDust = false;
                SpawnJumpDust();
            }

            if (_isSpawnFallDust)
            {
                _isSpawnFallDust = false;
                SpawnFallDust();
            }

            _animator.SetFloat(VerticalVelocityKey, _rigidbody.velocity.y);
            _animator.SetBool(IsRunningKey, _direction.x != 0);
            _animator.SetBool(IsGroundKey, _isGrounded);

            UpdateSpriteDirection();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.IsInLayer(_groundLayer))
            {
                var contacts = collision.contacts[0];
                if (contacts.relativeVelocity.y > _fallVelocity)
                {
                    _isSpawnFallDust = true;
                }
            }
        }
        #endregion


        private float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded) _allowDoubleJump = true;
            if (isJumpPressing)
            {
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.velocity.y > 0)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;
            if (!isFalling) return yVelocity;

            if (_isGrounded)
            {
                yVelocity += _jumpSpeed;
                _isSpawnJumpDust = true;
            }
            else if (_allowDoubleJump)
            {
                yVelocity = _jumpSpeed;
                _allowDoubleJump = false;
                _isSpawnJumpDust = true;
            }

            return yVelocity;
        }

        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        private bool IsGrounded()
        {
            return _groundCheck.IsTouchingLayer;
        }



        public void Collect(GameObject collectableObject)
        {
            if (collectableObject == null) return;

            var collectable = collectableObject.GetComponent<ICollectable>();
            if (collectable != null)
            {
                if (collectable is ScoreCollectable)
                {
                    _score += (collectable as ScoreCollectable).Score;
                }
            }

            Debug.Log($"Score: {_score}");
        }
        public void Interact()
        {
            var quantity = Physics2D.OverlapCircleNonAlloc(transform.position, _interactionRadius, _intractionResult, _interactionLayer);

            for (int i = 0; i < quantity; i++)
            {
                var component = _intractionResult[i].GetComponent<InteractableComponent>();
                if (component != null)
                {
                    component.Interact();
                }
            }
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public void TakeDamage()
        {
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);
        }

        public void SpawnFootDust()
        {
            _footStepsDust?.Spawn();
        }
        public void SpawnJumpDust()
        {
            _jumpDust?.Spawn();
        }
        public void SpawnFallDust()
        {
            _fallDust?.Spawn();
        }
    }
}