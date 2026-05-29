using System;
using IM.Factions;
using IM.Map;
using UnityEngine;

namespace IM.EntityIntelligence
{
    [CreateAssetMenu(menuName = "Entity Intelligence/Entity Actions/Seek Target In Room Entity Action Factory")]
    public class SeekTargetInRoomEntityActionFactory : EntityActionFactory
    {
        [SerializeField] private float _range = 5f;
        
        public override IEntityAction Create(GameObject param1)
        {
            return new SeekTargetInRoomEntityAction(param1.transform, 
                param1.GetComponent<IMemoryContainer>(),
                param1.GetComponent<IFactionMemberReadOnly>(),
                param1.GetComponent<IRoomVisitor>())
                {DetectionRange = _range};
        }
    }
}