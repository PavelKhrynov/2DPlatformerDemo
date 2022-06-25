﻿using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Creatures.Weapons
{
    public class BaseProjectile : MonoBehaviour
    {
        [SerializeField] protected float _speed;
        [SerializeField] protected bool _invertX;

        protected Rigidbody2D _rigidbody;
        protected float _direction;
        protected virtual void Start()
        {
            var mod = _invertX ? -1 : 1;
            _rigidbody = GetComponent<Rigidbody2D>();
            _direction = mod * transform.lossyScale.x > 0 ? 1 : -1;
        }
    }
}