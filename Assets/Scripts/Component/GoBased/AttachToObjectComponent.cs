using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Component.GoBased
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