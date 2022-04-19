using System.Collections;
using UnityEngine;

namespace WhereIAm.Scripts.Component
{
    public class AttachToObjectComponent : MonoBehaviour
    {
        public void Attach(GameObject parentObject)
        {
            transform.parent = parentObject.transform;
        }

        public void Detach()
        {
            transform.parent = null;
        }
    }
}