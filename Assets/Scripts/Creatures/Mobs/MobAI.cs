using Assets.Scripts.Component.ColliderBased;
using Assets.Scripts.Component.GoBased;
using Assets.Scripts.Creatures.Mobs.Patrolling;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Creatures.Mobs
{
    public class MobAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;

        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private float _missHeroCooldown = 0.5f;

        private static readonly int DieKey = Animator.StringToHash("is-dead");

        private Coroutine _current;
        private GameObject _target;

        private SpawnListComponent _particles;
        private Creature _creature;
        private Animator _animator;
        private Patrol _patrol;

        private bool _isDead = false;

        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
        }
        private void Start()
        {
            StartState(_patrol.DoPatrol());
        }


        public void OnHeroInVision(GameObject go)
        {
            if (_isDead) return;

            _target = go;

            StartState(AgroToHero());
        }
        public void OnDie()
        {
            _isDead = true;
            _animator.SetBool(DieKey, true);

            _creature.SetDirection(Vector2.zero);
            if (_current != null)
            {
                StopCoroutine(_current);
            }
        }

        private IEnumerator AgroToHero()
        {
            LookAtHero();
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);

            StartState(GoToHero());
        }
        private IEnumerator GoToHero()
        {
            while (_vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();
                }
                yield return null;
            }

            _creature.SetDirection(Vector2.zero);
            _particles.Spawn("MissHero");
            yield return new WaitForSeconds(_missHeroCooldown);

            StartState(_patrol.DoPatrol());
        }
        private IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }

            StartState(GoToHero());
        }

        private Vector2 GetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }
        private void LookAtHero()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(Vector2.zero);
            _creature.UpdateSpriteDirection(direction);
        }
        private void SetDirectionToTarget()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(direction);
        }
        private void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);
            if (_current != null)
            {
                StopCoroutine(_current);
            }
            _current = StartCoroutine(coroutine);
        }
    }
}