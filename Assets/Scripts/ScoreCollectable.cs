using System.Collections;
using UnityEngine;

namespace WhereIAm.Scripts
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