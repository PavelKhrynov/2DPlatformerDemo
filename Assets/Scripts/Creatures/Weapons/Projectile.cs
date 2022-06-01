﻿using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Creatures.Weapons
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Rigidbody2D _rigidbody;
        private float _direction;
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _direction = transform.lossyScale.x > 0 ? 1 : -1;
        }

        private void FixedUpdate()
        {
            var position = _rigidbody.position;
            position.x += _speed * _direction;

            _rigidbody.MovePosition(position);
        }
    }
}