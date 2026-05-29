using IM.Movement;
using UnityEngine;

namespace IM.EntityIntelligence
{
    [CreateAssetMenu(menuName = "Entity Intelligence/Entity Actions/Move Towards Target Factory")]
    public class MoveTowardsTargetFactory : EntityActionFactory
    {
        [SerializeField] private float _range = 3f; 
        [SerializeField] private float _resumeRange  = 4f;
        
        public override IEntityAction Create(GameObject param1)
        {
            return new MoveTowardsTarget(param1.transform, 
                    param1.GetComponent<IVectorMovement>(),
                    param1.GetComponent<IMemoryContainer>())
                {StopRange = _range,ResumeRange = _resumeRange};
        }
    }
}