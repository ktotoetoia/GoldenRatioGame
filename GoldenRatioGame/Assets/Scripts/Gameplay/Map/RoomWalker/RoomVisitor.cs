using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    [DisallowMultipleComponent]
    public class RoomVisitor : MonoBehaviour, IRoomVisitor, IRoomActivator
    {
        private IEntity _entity;
        
        [field: SerializeField] public bool ShouldActivate { get;private set; }
        public IRoom CurrentRoom { get; set; }
        public IEntity Entity => _entity??=GetComponent<IEntity>();

        public bool ActiveInRoom
        {
            get => gameObject.activeInHierarchy;
            set => gameObject.SetActive(value);
        }
    }
}