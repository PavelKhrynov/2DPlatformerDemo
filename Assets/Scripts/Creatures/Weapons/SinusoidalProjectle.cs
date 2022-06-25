using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Creatures.Weapons
{
    public class SinusoidalProjectle : BaseProjectile
    {
        [SerializeField] public float _frequency = 1f;
        [SerializeField] public float _amplitude = 1f;

        private float _originalY;
        private float _time;

        protected override void Start()
        {
            base.Start();

            _originalY = _rigidbody.position.y;
        }

        private void FixedUpdate()
        {
            var position = _rigidbody.position;
            position.x += _direction * _speed;
            position.y = _originalY +  Mathf.Sin(_time * _frequency) * _amplitude;
            _rigidbody.MovePosition(position);
            _time += Time.fixedDeltaTime;
        }
    }
}