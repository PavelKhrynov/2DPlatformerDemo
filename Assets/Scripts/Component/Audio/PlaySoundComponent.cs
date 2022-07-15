using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Component.Audio
{
    public class PlaySoundComponent : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioData[] _sounds;

        public void Play(string id)
        {
            foreach (var sound in _sounds)
            {
                if (sound.Id != id) continue;
                
                _source.PlayOneShot(sound.Clip);
                break;
            }
        }

        [Serializable]
        public class AudioData
        {
            [SerializeField] private string _id;
            [SerializeField] private AudioClip _clip;

            public string Id => _id;
            public AudioClip Clip => _clip;
        }
    }
}