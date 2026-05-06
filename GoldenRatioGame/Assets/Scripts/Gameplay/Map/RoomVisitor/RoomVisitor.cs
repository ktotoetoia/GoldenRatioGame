using UnityEngine;

namespace IM.Map
{
    [DisallowMultipleComponent]
    public class RoomVisitor : MonoBehaviour, IRoomVisitor, IRoomActivator
    {
        private IGameObjectRoom _currentRoom;
        [field: SerializeField] public bool ShouldActivate { get;private set; }

        public IGameObjectRoom CurrentRoom { get; set; }

        private void OnTransformParentChanged()
        {
            if (!transform.parent && CurrentRoom is MonoBehaviour mb)
            {
                transform.SetParent(mb.transform);
            }
        }
    }
}