using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Animations
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] private int _frameRate = 8;
        [SerializeField] private AnimationState[] _stateList;
        [SerializeField] private UnityEvent _onComplete;
        [SerializeField] private bool _dontPlayOnStart = false;

        private SpriteRenderer _renderer;
        private float _secondsPerFrame;
        private int _currentSpriteIndex;
        private int _currentStateIndex;
        private float _nextFrameTime;
        private Sprite[] _sprites;

        private bool _isLoop = false;
        private bool _isPlaying = false;
        private bool _isPlayNext = false;

        private void SetState(AnimationState state)
        {
            _isPlaying = false;
            if (state != null)
            {
                _sprites = state.Sprites;
                _isLoop = state.IsLoop;
                _isPlayNext = state.IsPlayNext;
            }
            _currentSpriteIndex = 0;
            _nextFrameTime = Time.time;
            _isPlaying = true;
        }


        public void SetClip(string name)
        {
            for (int i = 0; i < _stateList.Length; i++)
            {
                var stateToCheck = _stateList[i];
                if (stateToCheck.Name == name)
                {
                    _currentStateIndex = i;
                    SetState(stateToCheck);
                    break;
                }
            }
        }


        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _secondsPerFrame = 1f / _frameRate;
            _nextFrameTime = Time.time + _secondsPerFrame;
            _currentStateIndex = 0;

            if (_stateList != null && _stateList.Length > 0 && !_dontPlayOnStart)
            {
                SetState(_stateList[_currentStateIndex]);
            }
        }

        private void Update()
        {
            if (!_isPlaying || _nextFrameTime > Time.time) return;

            if (_currentSpriteIndex >= _sprites.Length)
            {
                if (_isPlayNext)
                {
                    _stateList[_currentStateIndex].OnComplete?.Invoke();
                    _currentStateIndex = _currentStateIndex >= _stateList.Length - 1 ? 0 : _currentStateIndex + 1;
                    SetState(_stateList[_currentStateIndex]);
                }
                else if (_isLoop)
                {
                    _currentSpriteIndex = 0;
                }
                else
                {
                    _isPlaying = false;
                    _stateList[_currentStateIndex].OnComplete?.Invoke();
                    _onComplete?.Invoke();
                }
            }

            if (_isPlaying)
            {
                _renderer.sprite = _sprites[_currentSpriteIndex];
                _nextFrameTime += _secondsPerFrame;
                _currentSpriteIndex++;
            }
        }
    }
}
