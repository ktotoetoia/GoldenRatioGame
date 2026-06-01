using System;
using IM.Factions;
using IM.Map;
using UnityEngine;

namespace IM.EntityIntelligence
{
    [Serializable]
    public class SeekTargetInRoomEntityActionFactory : IEntityActionFactory
    {
        [SerializeField] private float _range = 5f;
        
        public IEntityAction Create(GameObject param1)
        {
            return new SeekTargetInRoomEntityAction(param1.transform, 
                param1.GetComponent<IMemoryContainer>(),
                param1.GetComponent<IFactionMemberReadOnly>(),
                param1.GetComponent<IRoomVisitor>())
                {DetectionRange = _range};
        }
    }
}