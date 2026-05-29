using UnityEngine;

namespace IM.EntityIntelligence
{
    public class TargetMemory : IMemory
    {
        public GameObject Target { get; set; }
        public bool IsSeen { get; set; }
        public Vector3 LastSeenAt { get; set; }
        
        public void Clear()
        {
            Target = null;
            IsSeen = false;
            LastSeenAt = Vector3.zero;
        }
    }
}