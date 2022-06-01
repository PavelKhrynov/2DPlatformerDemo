using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Assets.Scripts.Utils;

namespace Assets.Scripts.Component.ColliderBased
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private string[] _tags;
        [SerializeField] private UnityEvent<GameObject> _onOverlapEvent;

        private Collider2D[] _intractionResult = new Collider2D[10];

        public void Check()
        {
            var quantity = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _intractionResult, _mask);

            for (int i = 0; i < quantity; i++)
            {
                var overlapResult = _intractionResult[i];
                var isInTag = _tags.Length == 0 || _tags.Any(tag => overlapResult.CompareTag(tag));
                if (isInTag)
                {
                    _onOverlapEvent?.Invoke(overlapResult.gameObject);
                }
            }
        }


        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }
    }
}