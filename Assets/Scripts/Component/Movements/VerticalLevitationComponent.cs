using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Component.Movements
{
    public class VerticalLevitationComponent : MonoBehaviour
    {
        [SerializeField] public float _frequency = 1f;
        [SerializeField] public float _amplitude = 1f;
        [SerializeField] public bool _randomize;

        private float _originalY;
        private Rigidbody2D _rigidbody;
        private float _seed;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _originalY = _rigidbody.transform.position.y;

            if (_randomize)
            {
                _seed = Random.value * Mathf.PI * 2;
            }
        }

        private void Update()
        {
            var pos = _rigidbody.position;
            pos.y = _originalY + Mathf.Sin(_seed + Time.time * _frequency) * _amplitude;
            _rigidbody.MovePosition(pos);
        }
    }
}