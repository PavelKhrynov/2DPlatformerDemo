using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WhereIAm.Scripts.Component
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;

        public void Interact()
        {
            _action?.Invoke();
        }
    }
}
