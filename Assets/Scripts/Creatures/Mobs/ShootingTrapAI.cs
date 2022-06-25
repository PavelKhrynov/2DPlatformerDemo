using Assets.Scripts.Component.ColliderBased;
using Assets.Scripts.Component.GoBased;
using Assets.Scripts.Utils;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Creatures.Mobs
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;


        [Header("Melee")]
        [SerializeField] private Cooldown _meleeCooldown;
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private LayerCheck _meleeCanAttack;

        [Header("Range")]
        [SerializeField] private Cooldown _rangeCooldown;
        [SerializeField] private SpawnComponent _rangeAttack;

        private Animator _animator;
        private static readonly int Melee = Animator.StringToHash("melee");
        private static readonly int Range = Animator.StringToHash("range");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if(_vision.IsTouchingLayer)
            {
                if(_meleeCanAttack.IsTouchingLayer)
                {
                    if(_meleeCooldown.IsReady)
                    {
                        MeleeAttack();
                    }
                    return;
                }

                if (_rangeCooldown.IsReady)
                {
                    RangeAttack();
                }
            }
        }

        private void RangeAttack()
        {
            _rangeCooldown.Reset();
            _animator.SetTrigger(Range);
        }

        private void MeleeAttack()
        {
            _meleeCooldown.Reset();
            _animator.SetTrigger(Melee);
        }
        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
        }

        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }
    }
}