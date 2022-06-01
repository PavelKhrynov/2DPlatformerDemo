using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Collectables
{
    public class ScoreCollectable : MonoBehaviour, ICollectable
    {
        [SerializeField] private int _score = 0;

        public int Score
        {
            get { return _score; }
        }
    }
}