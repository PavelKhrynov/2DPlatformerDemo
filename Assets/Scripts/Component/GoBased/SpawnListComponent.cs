using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Component.GoBased
{
    public class SpawnListComponent : MonoBehaviour
    {
        [SerializeField] private SpawnData[] _spawners;

        public void Spawn(string id)
        {
            var spawn = _spawners.FirstOrDefault(x => x.Id == id);
            spawn?.Component.Spawn();
        }

        [Serializable]
        public class SpawnData
        {
            public string Id;
            public SpawnComponent Component;
        }
    }
}