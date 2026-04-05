using UnityEngine;

namespace IM.Map
{
    [DisallowMultipleComponent]
    public class RoomVisitor : MonoBehaviour, IRoomVisitor, IRoomActivator
    {
        [field: SerializeField] public bool ShouldActivate { get;private set; }
        public IRoom CurrentRoom { get; set; }

        public bool ActiveInRoom
        {
            get => gameObject.activeInHierarchy;
            set => gameObject.SetActive(value);
        }
    }
}