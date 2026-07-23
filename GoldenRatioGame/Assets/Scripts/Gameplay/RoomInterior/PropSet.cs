using System;
using UnityEngine;

namespace IM.Map
{
    [Serializable]
    public class PropSet
    {
        [field:SerializeField] public PropInfo PropInfo { get; private set; }
        [field: SerializeField] public int MinCount { get; private set; } = 0;
        [field: SerializeField] public int MaxCount { get; private set; } = 2;
        
        [field:SerializeReferenceDropdown] [field:SerializeReference] public IPropPlacementStrategy PropPlacementStrategy { get; private set; }
    }
}