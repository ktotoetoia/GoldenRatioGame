using System;
using IM.Common;
using IM.Factions;
using UnityEngine;

namespace IM
{
    public class FactionEnterTemporary : MonoBehaviour, ITemporary
    {
        [SerializeField] private float _toFinishTime = 2;
        [SerializeField] private bool _finishOnTriggerEnterAlly;
        [SerializeField] private bool _finishOnTriggerEnterEnemy;
        [SerializeField] private bool _finishOnTriggerEnterNone;
        private float _initializationTime;
        private Action _onFinished;
        private bool _finished;

        private void Awake()
        {
            IFactionCollisionDetector collisionDetector = GetComponent<IFactionCollisionDetector>();

            collisionDetector.OnTriggerEnterNone += x =>
            {
                if (_finishOnTriggerEnterNone) Finish();
            };
            collisionDetector.OnTriggerEnterAlly += x =>
            {
                if (_finishOnTriggerEnterAlly) Finish();
            };
            collisionDetector.OnTriggerEnterEnemy += x =>
            {
                if (_finishOnTriggerEnterEnemy) Finish();
            };
        }
        
        private void Update()
        {
            if (_finished || !(Time.time > _initializationTime + _toFinishTime)) return;
            
            Finish();
        }

        private void Finish()
        {
            if(_finished) return;;
            
            _finished = true;
            _onFinished?.Invoke();
        }

        public void Initialize(Action onFinished)
        {
            _initializationTime = Time.time;
            _onFinished = onFinished;
            _finished = false;
        }
    }
}