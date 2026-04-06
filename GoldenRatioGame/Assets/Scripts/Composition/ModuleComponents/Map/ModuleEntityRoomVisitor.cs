using IM.Items;
using IM.LifeCycle;
using IM.Modules;
using UnityEngine;

namespace IM.Map
{
    public class ModuleEntityRoomVisitor : MonoBehaviour, IRoomVisitor, IItemDropObserver, IRoomActivator
    {
        private IEntity _entity;
        
        [field: SerializeField] public bool ShouldActivate { get;private set; }
        public IEntity Entity => _entity ??= GetComponent<IEntity>(); 
        public IRoom CurrentRoom { get; set; }
        public bool ActiveInRoom
        {
            get => gameObject.activeInHierarchy;
            set => gameObject.SetActive(value);
        }

        public void OnItemDropped(IItem item)
        {
            if (CurrentRoom != null && item is MonoBehaviour mb && mb.TryGetComponent(out IRoomVisitor roomVisitor))
            {
                CurrentRoom.Add(roomVisitor);
            }
        }
    }
}