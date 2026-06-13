using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Factions
{
    [CreateAssetMenu(menuName = "Factions/Faction")]
    public class Faction : ScriptableObject, IFaction, IEquatable<Faction>
    {
        [SerializeField] private string _id = "Faction ID";
        [SerializeField] private FactionRelation _defaultRelation;
        [SerializeField] private List<FactionRelationOverride> _customRelationships;
        
        public FactionRelation GetRelationWith(IFaction other)
        {
            if (other is not Faction otherFaction)
                return _defaultRelation;

            if (otherFaction == this)
                return FactionRelation.Ally;

            FactionRelationOverride match = _customRelationships
                .FirstOrDefault(rel => rel.TargetFaction == otherFaction);

            return match?.Relation ?? _defaultRelation;
        }

        public override bool Equals(object other)
        {
            return other is Faction faction && Equals(faction);
        }

        public bool Equals(Faction other)
        {
            return _id.Equals(other?._id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), _id, (int)_defaultRelation, _customRelationships);
        }
        
        [Serializable]
        private class FactionRelationOverride
        {
            [field: SerializeField] public Faction TargetFaction { get; set; }
            [field: SerializeField] public FactionRelation Relation { get; set; }
        }
    }
}