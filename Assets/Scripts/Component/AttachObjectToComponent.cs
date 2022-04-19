using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Component
{
    public class AttachObjectToComponent : MonoBehaviour
    {

        public void Attach(GameObject parentObject)
        {
            parentObject.transform.parent = transform;
        }

        public void Detach(GameObject parentObject)
        {
            parentObject.transform.parent = null;
        }
    }
}