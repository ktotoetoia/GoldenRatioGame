using System;
using IM.LifeCycle;
using UnityEngine;

namespace IM
{
    public class ReturnToPoolHitContract : HitContractBase, ITemporary
    {
        [SerializeField] private float _toFinishTime = 2f;
        [SerializeField] private bool _finishOnDisable = true;
        private float _initializationTime;
        private Action _onFinished;
        private bool _finished;

        private void Update()
        {
            if (_finished || Time.time <= _initializationTime + _toFinishTime) return;
        
            Finish();
        }

        protected override void ProcessHit(GameObject target)
        {
            Finish();
        }

        private void Finish()
        {
            if (_finished) return;
        
            _finished = true;
            _onFinished?.Invoke();
        }

        public void Initialize(Action onFinished)
        {
            _initializationTime = Time.time;
            _onFinished = onFinished;
            _finished = false;
        }

        private void OnDisable()
        {
            if (_finishOnDisable) Finish();
        }
    }
}