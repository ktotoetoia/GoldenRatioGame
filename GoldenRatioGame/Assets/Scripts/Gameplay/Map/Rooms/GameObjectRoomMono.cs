using System;
using System.Collections.Generic;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    public class GameObjectRoomMono : MonoBehaviour, IRoom
    {
        private readonly GameObjectRoom _room = new ();

        public bool IsActive => _room.IsActive;

        public IEnumerable<IRoomVisitor> RoomVisitors => _room.RoomVisitors;
        public IEnumerable<IRoomWalker> RoomWalkers => _room.RoomWalkers;

        private void Awake()
        {
            UpdateActive(_room.IsActive);
        }

        public bool Add(IRoomVisitor roomVisitor)
        {
            bool added = _room.Add(roomVisitor);
            
            if (roomVisitor is MonoBehaviour mb && added)
            {
                mb.transform.SetParent(transform);
            }
            return added;
        } 
        public bool Remove(IRoomVisitor roomVisitor)
        {
            bool removed = _room.Remove(roomVisitor);
            
            if (roomVisitor is MonoBehaviour mb && removed && mb.TryGetComponent(out IParentRestorable r))
            {
                r.ResetToDefaultParent();
            }

            return removed;
        }

        public void Enter(IRoomWalker roomWalker)
        {
            _room.Enter(roomWalker);  
            UpdateActive(_room.IsActive);
        }

        public void Exit(IRoomWalker roomWalker)
        {
            _room.Exit(roomWalker);  
            UpdateActive(_room.IsActive);
        } 

        private void UpdateActive(bool value)
        {
            gameObject.SetActive(value);
        }
    } 
}