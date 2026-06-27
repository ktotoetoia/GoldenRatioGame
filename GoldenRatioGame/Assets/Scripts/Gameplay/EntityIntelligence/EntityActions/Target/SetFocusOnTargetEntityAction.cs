using IM.Values;
using UnityEngine;

namespace IM.EntityIntelligence
{
    public class SetFocusOnTargetEntityAction : EntityAction
    {
        private readonly Transform _ownerTransform;
        private readonly IFocusDirectionOverrider _focusDirectionOverrider;
        private readonly TargetMemory _targetMemory;

        public float FocusDuration { get; set; } = 0.5f;
        
        public SetFocusOnTargetEntityAction(Transform ownerTransform, IMemoryContainer memoryContainer, IFocusDirectionOverrider focusDirectionOverrider)
        {
            _ownerTransform = ownerTransform;
            _focusDirectionOverrider = focusDirectionOverrider;

            if (memoryContainer.Memories.TryGet(out _targetMemory)) return;
           
            _targetMemory = new TargetMemory();
            memoryContainer.Add(_targetMemory);

        }

        public override void Update()
        {
            if(!_targetMemory.IsSeen)return;
           
            _focusDirectionOverrider?.OverrideFocusDirection(_targetMemory.Target.transform.position- _ownerTransform.position,FocusDuration);
        }
    }
}