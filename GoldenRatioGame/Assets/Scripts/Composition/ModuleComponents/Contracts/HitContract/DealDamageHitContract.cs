using System;
using IM.Factions;
using IM.Health;
using UnityEngine;

namespace IM
{
    public class DealDamageHitContract : MonoBehaviour
    {
        [SerializeField] private bool _finishOnTriggerEnterAlly;
        [SerializeField] private bool _finishOnTriggerEnterEnemy;
        [SerializeField] private bool _finishOnTriggerEnterNone;
        private IDamageDealer _damageDealer;
        private float _initializationTime;
        private Action _onFinished;
        private bool _finished;
        
        private void Awake()
        {
            _damageDealer = GetComponent<IDamageDealer>();
            IFactionCollisionDetector collisionDetector = GetComponent<IFactionCollisionDetector>();

            collisionDetector.OnTriggerEnterNone += x =>
            {
                if (_finishOnTriggerEnterNone) DealDamage(x);
            };
            collisionDetector.OnTriggerEnterAlly += x =>
            {
                if (_finishOnTriggerEnterAlly) DealDamage(x);
            };
            collisionDetector.OnTriggerEnterEnemy += x =>
            {
                if (_finishOnTriggerEnterEnemy) DealDamage(x);
            };
        }

        private void DealDamage(GameObject go)
        {
            if (!go.TryGetComponent(out IHealth health)) return;
            
            _damageDealer.DealDamage(health);
        }
    }
}