using System.Collections.Generic;
using UnityEngine;

namespace IM.Factions
{
    [CreateAssetMenu(menuName = "Factions/Faction Database")]
    public class FactionDatabase : ScriptableObject, IFactionDatabase
    {
        [SerializeField] private List<Faction> _factions;
        
        public string GetIdOf(IFaction faction)
        {
            if (faction is not Faction f) return "";
            
            return _factions.IndexOf(f).ToString();
        }

        public IFaction GetById(string id)
        {
            if (!int.TryParse(id, out int result)) return null;
            
            return _factions[result];
        }
    }
}