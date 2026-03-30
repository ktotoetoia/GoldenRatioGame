using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    public class GameObjectRoomMono : MonoBehaviour, IRoom
    {
        private GameObjectRoom _room;
        
        public IReadOnlyCollection<GameObject> GameObjects => _room.GameObjects;

        private void Awake()
        {
            _room = new GameObjectRoom();
            _room.GameObjectAdded += x => x.transform.SetParent(transform);
            _room.GameObjectRemoved += x =>  x.transform.SetParent(null);
        }

        public void Add(IRoomVisitor roomVisitor)
        {
            _room.Add(roomVisitor);
        }

        public void Remove(IRoomVisitor roomVisitor)
        {
            _room.Remove(roomVisitor);
        }

        public void Enter(IRoomWalker roomWalker)
        {
            _room.Enter(roomWalker);
        }

        public void Exit(IRoomWalker roomWalker)
        {
            _room.Exit(roomWalker);
        }
    } 
}