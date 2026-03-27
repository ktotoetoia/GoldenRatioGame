using System;
using System.Collections.Generic;
using IM.Factions;
using UnityEngine;

namespace IM
{
    [Serializable]
    public class ModuleEntityEntry
    {
        [field:SerializeField] public Bounds Bounds { get; private set; }
        [field:SerializeField] public GameObject EntityGameObject{ get; private set; }
        [field:SerializeField] public List<GameObject> ModulesGameObjects{ get; private set; }
        [field:SerializeField] public Faction Faction{ get; private set; }
    }
}