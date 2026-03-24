using System;
using IM.Factions;
using IM.SaveSystem;
using UnityEngine;

namespace IM.Modules
{
    public class FactionMemberSerializer : ComponentSerializer<FactionMember>
    {
        private readonly IFactionDatabase _factionDatabase;

        public FactionMemberSerializer(IFactionDatabase factionDatabase)
        {
            _factionDatabase = factionDatabase;
        } 
        
        public override object CaptureState(FactionMember component)
        {
            return _factionDatabase.GetIdOf(component.Faction);
        }

        public override void RestoreState(FactionMember component, object state, Func<string, GameObject> resolveDependency)
        {
            component.Faction = _factionDatabase.GetById((string)state);
        }
    }
}