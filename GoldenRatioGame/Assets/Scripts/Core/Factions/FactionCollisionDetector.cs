using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Factions
{
    public class FactionCollisionDetector : MonoBehaviour, IFactionCollisionDetector, IRequireFactionMember
    {
        [SerializeField] private bool _triggerOnSameTarget;
        private readonly List<GameObject> _triggeredOn = new();
        
        public IFactionMemberReadOnly FactionMemberReadOnly { get; set; }

        public event Action<GameObject> OnTriggerEnterEnemy;
        public event Action<GameObject> OnTriggerEnterAlly;
        public event Action<GameObject> OnTriggerEnterNone;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IEnvironmentObject environmentObject))
            {
                OnTriggerEnterNone?.Invoke(other.gameObject);
                return;
            }
            
            if (FactionMemberReadOnly?.Faction == null || 
                !other.TryGetComponent(out IFactionMemberReadOnly factionMember) || 
                factionMember?.Faction == null || _triggeredOn.Contains(gameObject)) return;
            
            _triggeredOn.Add(other.gameObject);
            
            switch (FactionMemberReadOnly.Faction.GetRelationWith(factionMember.Faction))
            {
                case FactionRelation.Ally:
                    OnTriggerEnterAlly?.Invoke(other.gameObject);
                    return;
                case FactionRelation.Enemy:
                    OnTriggerEnterEnemy?.Invoke(other.gameObject);
                    return;
                case FactionRelation.None:
                    OnTriggerEnterNone?.Invoke(other.gameObject);
                    return;
            }
        }
        
        private void OnEnable() => _triggeredOn.Clear();
    }
}