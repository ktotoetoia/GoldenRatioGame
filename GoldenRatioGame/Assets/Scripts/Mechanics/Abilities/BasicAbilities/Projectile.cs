using System;
using UnityEngine;

namespace IM.Abilities
{
    public class Projectile : MonoBehaviour, IProjectile
    {
        [SerializeField] private float _toFinishTime = 2;
        private float _initializationTime;
        private Action _onFinished;
        private bool _finished;
        
        private void Update()
        {
            if (!_finished && Time.time > _initializationTime + _toFinishTime)
            {
                _onFinished?.Invoke();
                _finished = true;
            }
        }

        public void Initialize(Action onFinished)
        {
            _initializationTime = Time.time;
            _onFinished = onFinished;
            _finished = false;
        }
    }
}