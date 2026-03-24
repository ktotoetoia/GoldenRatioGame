using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Factions
{
    [CreateAssetMenu(menuName = "Factions/Faction")]
    public class Faction : ScriptableObject, IFaction
    {
        [SerializeField] private FactionRelation _defaultRelation;
        [SerializeField] private List<FactionRelationOverride> _customRelationships;
    
        public FactionRelation GetRelationWith(IFaction other)
        {
            if (Equals(other)) return FactionRelation.Ally;
            
            FactionRelationOverride match = _customRelationships.FirstOrDefault(rel => rel.TargetFaction.Equals(other));
            return match?.Relation ?? _defaultRelation;
        }
        
        [Serializable]
        private class FactionRelationOverride
        {
            [field: SerializeField] public Faction TargetFaction { get; set; }
            [field: SerializeField] public FactionRelation Relation { get; set; }
        }
    }
}