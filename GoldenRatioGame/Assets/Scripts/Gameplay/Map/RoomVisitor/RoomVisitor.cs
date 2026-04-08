using UnityEngine;

namespace IM.Map
{
    [DisallowMultipleComponent]
    public class RoomVisitor : MonoBehaviour, IRoomVisitor, IRoomActivator
    {
        [field: SerializeField] public bool ShouldActivate { get;private set; }
        public IGameObjectRoom CurrentRoom { get; set; }
    }
}