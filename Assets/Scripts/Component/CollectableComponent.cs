using Assets.Scripts.Creatures.Hero;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Component
{
    public class CollectableComponent : MonoBehaviour
    {
        public void Collect(GameObject target)
        {
            var creature = target.GetComponent<Hero>();
            if (creature != null)
            {
                creature.Collect(gameObject);
            }
        }
    }
}