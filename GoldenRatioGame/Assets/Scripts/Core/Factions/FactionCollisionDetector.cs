using System;
using UnityEngine;

namespace IM.Factions
{
    public class FactionCollisionDetector : MonoBehaviour, IFactionCollisionDetector, IRequireFactionMember
    {
        public IFactionMember FactionMember { get; set; }

        public event Action<GameObject> OnTriggerEnterEnemy; 
        public event Action<GameObject> OnTriggerEnterAlly; 
        public event Action<GameObject> OnTriggerEnterNone; 
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IFactionMember factionMember))
            {
                switch (FactionMember.Faction.Get(factionMember.Faction))
                {
                    case FactionRelation.None:
                        OnTriggerEnterNone?.Invoke(other.gameObject);
                        break;
                    case FactionRelation.Ally:
                        OnTriggerEnterAlly?.Invoke(other.gameObject);
                        break;
                    case FactionRelation.Enemy:
                        OnTriggerEnterEnemy?.Invoke(other.gameObject);
                        break;
                }
            }
        }
    }
}