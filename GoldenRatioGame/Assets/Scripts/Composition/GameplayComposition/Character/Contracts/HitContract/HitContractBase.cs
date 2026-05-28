using IM.Factions;
using UnityEngine;

namespace IM
{
    public abstract class HitContractBase : MonoBehaviour
    {
        [SerializeField] private bool _triggerOnAlly;
        [SerializeField] private bool _triggerOnEnemy;
        [SerializeField] private bool _triggerOnNone;
        private IFactionCollisionDetector _collisionDetector;

        protected virtual void Awake()
        {
            _collisionDetector = GetComponent<IFactionCollisionDetector>();
        
            if (_collisionDetector == null)
            {
                Debug.LogWarning($"{name} is missing an IFactionCollisionDetector!", this);
                return;
            }

            _collisionDetector.OnTriggerEnterNone += HandleTriggerNone;
            _collisionDetector.OnTriggerEnterAlly += HandleTriggerAlly;
            _collisionDetector.OnTriggerEnterEnemy += HandleTriggerEnemy;
        }

        protected virtual void OnDestroy()
        {
            if (_collisionDetector == null) return;

            _collisionDetector.OnTriggerEnterNone -= HandleTriggerNone;
            _collisionDetector.OnTriggerEnterAlly -= HandleTriggerAlly;
            _collisionDetector.OnTriggerEnterEnemy -= HandleTriggerEnemy;
        }

        private void HandleTriggerNone(GameObject target) { if (_triggerOnNone) ProcessHit(target); }
        private void HandleTriggerAlly(GameObject target) { if (_triggerOnAlly) ProcessHit(target); }
        private void HandleTriggerEnemy(GameObject target) { if (_triggerOnEnemy) ProcessHit(target); }

        protected abstract void ProcessHit(GameObject target);
    }
}