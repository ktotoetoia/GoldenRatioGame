using UnityEngine;

namespace IM.Factions
{
    public class FactionMember : MonoBehaviour, IFactionMember
    {
        public IFaction Faction { get; set; }
    }
}