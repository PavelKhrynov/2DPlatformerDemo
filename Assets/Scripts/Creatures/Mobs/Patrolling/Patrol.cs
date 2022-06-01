using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Creatures.Mobs.Patrolling
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();
    }
}