using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Component.ColliderBased
{
    public class ExitCollisionComponent : MonoBehaviour
    {

        [SerializeField] private string _tag;
        [SerializeField] private UnityEvent<GameObject> _action;

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(_tag))
            {
                _action?.Invoke(collision.gameObject);
            }
        }
    }
}