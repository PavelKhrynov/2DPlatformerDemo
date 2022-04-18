using System.Collections;
using UnityEngine;

namespace WhereIAm.Scripts.Component
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destination;

        public void Teleport(GameObject target)
        {
            target.transform.position = _destination.position;
        }
    }
}