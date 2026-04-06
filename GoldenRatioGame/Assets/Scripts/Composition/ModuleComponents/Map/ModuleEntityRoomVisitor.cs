using IM.Items;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    public class ModuleEntityRoomVisitor : MonoBehaviour, IRoomVisitor, IRoomActivator
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
    }
}