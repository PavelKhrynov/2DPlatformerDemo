using Assets.Scripts.Component.ColliderBased;
using Assets.Scripts.Component.GoBased;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] private bool _invertScale;
        [SerializeField] private float _speed;
        [SerializeField] protected float _jumpSpeed;
        [SerializeField] private float _damageVelocity;
        [SerializeField] protected int _damage;
        [SerializeField] private float _slopeCheckDistance = 0.5f;

        [Header("Checkers")]
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] protected CheckCircleOverlap _attackRange;

        [SerializeField] protected SpawnListComponent _particles;

        [Header("Material")]
        [SerializeField] private PhysicsMaterial2D noFriction;
        [SerializeField] private PhysicsMaterial2D fullFriction;

        protected Rigidbody2D _rigidbody;
        protected Vector2 _direction;
        protected Animator _animator;
        private CapsuleCollider2D _capsuleCollider;

        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int IsRunningKey = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        protected static readonly int AttackKey = Animator.StringToHash("attack");

        protected bool _isGrounded;
        protected bool _isSpawnJumpDust;

        private Vector2 _colliderSize;
        private Vector2 _slopeNormalPerpendicular;
        private float _slopeDownAngle;
        private float _slopeDownAngleOld;
        private float _slopeSideAngle;
        private bool _isOnSlope;

        #region Unity Functions
        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _capsuleCollider = GetComponent<CapsuleCollider2D>();

            _colliderSize = _capsuleCollider.size;
        }
        private void Update()
        {
            _isGrounded = IsGrounded();
        }

        protected virtual void FixedUpdate()
        {
            var xVelocity = CalculateXVelocity();
            var yVelocity = CalculateYVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            if (_isSpawnJumpDust)
            {
                _isSpawnJumpDust = false;
                _particles.Spawn("Jump");
            }

            _animator.SetFloat(VerticalVelocityKey, _rigidbody.velocity.y);
            _animator.SetBool(IsRunningKey, _direction.x != 0);
            _animator.SetBool(IsGroundKey, _isGrounded);

            UpdateSpriteDirection(_direction);
            SlopeCheck();
        }
        #endregion

        private float CalculateXVelocity()
        {
            var xVelocity = _direction.x * _speed;

            if (_isGrounded && _isOnSlope)
            {
                xVelocity *= _slopeNormalPerpendicular.x * -1;
            }

            return xVelocity;
        }
        protected virtual float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumping = _direction.y > 0;
            var isFalling = yVelocity <= 0.001f;

            if (isJumping)
            {
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (_rigidbody.velocity.y > 0)
            {
                yVelocity *= 0.5f;
            }

            if (_isGrounded && _isOnSlope)
            {
                yVelocity *= _slopeNormalPerpendicular.y * -1;
            }

            return yVelocity;
        }

        private bool IsGrounded()
        {
            return _groundCheck.IsTouchingLayer;
        }
        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (_isGrounded)
            {
                _isSpawnJumpDust = true;
                yVelocity += _jumpSpeed;
            }

            return yVelocity;
        }
        private void SlopeCheck()
        {
            Vector2 checkPosition = transform.position - new Vector3(0.0f, _colliderSize.y / 2);

            SlopeCheckHorizontal(checkPosition);
            SlopeCheckVertical(checkPosition);
        }
        private void SlopeCheckHorizontal(Vector2 checkPosition)
        {
            RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPosition, transform.right, _slopeCheckDistance, _groundLayer);
            RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPosition, -transform.right, _slopeCheckDistance, _groundLayer);

            if (slopeHitFront)
            {
                _isOnSlope = true;
                _slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
            }
            else if (slopeHitBack)
            {
                _isOnSlope = true;
                _slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
            }
            else
            {
                _slopeSideAngle = 0.0f;
                _isOnSlope = false;
            }
        }
        private void SlopeCheckVertical(Vector2 checkPosition)
        {
            RaycastHit2D hit = Physics2D.Raycast(checkPosition, Vector2.down, _slopeCheckDistance, _groundLayer);

            if (hit)
            {
                _slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized;
                _slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (_slopeDownAngle != _slopeDownAngleOld)
                {
                    _isOnSlope = true;
                    _slopeDownAngleOld = _slopeDownAngle;
                }

                Debug.DrawRay(hit.point, _slopeNormalPerpendicular, Color.red);
                Debug.DrawRay(checkPosition, hit.normal, Color.green);
            }

            if (_isOnSlope && _direction.x <= float.Epsilon)
            {
                _rigidbody.sharedMaterial = fullFriction;
            }
            else
            {
                _rigidbody.sharedMaterial = noFriction;
            }
        }

        public void UpdateSpriteDirection(Vector2 direction)
        {
            var multiplier = _invertScale ? -1 : 1;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);
            }
        }
        public virtual void Attack()
        {
            _animator.SetTrigger(AttackKey);
        }
        public void OnAnimationAttack()
        {
            _attackRange.Check();
        }
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }
        public void TakeDamage()
        {
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageVelocity);
        }
    }
}