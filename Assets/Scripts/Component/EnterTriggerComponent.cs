using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WhereIAm.Scripts.Component
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private UnityEvent _action;
        [SerializeField] private UnityEvent<GameObject> _actionWithObject;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(_tag))
            {
                _action?.Invoke();
                _actionWithObject?.Invoke(collision.gameObject);
            }
        }
    }
}

