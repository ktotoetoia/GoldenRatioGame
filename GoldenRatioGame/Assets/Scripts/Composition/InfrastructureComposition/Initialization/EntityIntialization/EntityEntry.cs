using System;
using IM.Factions;
using UnityEngine;

namespace IM
{
    [Serializable]
    public class EntityEntry
    {
        [field:SerializeField] public Bounds Bounds { get; private set; }
        [field:SerializeField] public GameObject EntityGameObject{ get; private set; }
        [field:SerializeField] public Faction Faction{ get; private set; }
    }
}