using System;
using IM.Entities;
using IM.LifeCycle;
using IM.Map;
using IM.Modules;
using UnityEngine;

namespace IM.UI
{
    public class DisplayGameObjectsInPlayerRoom : MonoBehaviour, IRequirePlayerEntity
    {
        private IRoomVisitor _roomVisitor;
        private CollectionDiffer<GameObject> _rooms;
        private IGameObjectStatusDisplayCollection _collection; 
        
        private void Awake()
        {
            _collection =  GetComponent<IGameObjectStatusDisplayCollection>();
            _rooms = new CollectionDiffer<GameObject>(x =>
            {
                if(x.TryGetComponent(out IModuleEntity _)) _collection.Add(x);
            }, x =>
            {
                _collection.Remove(x);
            });
        }
        
        public void SetPlayerEntity(IEntity playerEntity)
        {
            playerEntity?.GameObject?.TryGetComponent(out _roomVisitor);
            
        }

        private void LateUpdate()
        {
            if (_roomVisitor?.CurrentRoom == null)
            {
                _rooms.Update(Array.Empty<GameObject>());
                
                return;
            } 
            
            _rooms.Update(_roomVisitor.CurrentRoom.GameObjects);
        }
    }
}