using System;
using System.Collections.Generic;
using System.Linq;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    public class GameObjectRoom : IRoom
    {
        private readonly IRoomInitializer _roomInitializer;
        private readonly Dictionary<GameObject, IPausable> _toPause = new();
        private SmartGameObjectCollection _gameObjectCollection;
        private bool _firstTimeEnter = true;
        
        public IReadOnlyCollection<GameObject> GameObjects => _gameObjectCollection;
        
        public event Action<GameObject> GameObjectAdded;
        public event Action<GameObject> GameObjectRemoved;

        public GameObjectRoom() : this(new GameObject[] {})
        {
            
        }
        
        public GameObjectRoom(IEnumerable<GameObject> gameObjects)
        {
            TryInitialize(gameObjects);
        }
        
        public GameObjectRoom(IRoomInitializer roomInitializer)
        {
            _roomInitializer = roomInitializer;
        }

        public void Add(IRoomVisitor roomVisitor)
        {
            if(roomVisitor is not MonoBehaviour mo || _gameObjectCollection.Contains(mo.gameObject)) return;
            
            foreach (IRoomVisitor rv in mo.gameObject.GetComponentsInChildren<IRoomVisitor>()
                         .Where(x=> !_gameObjectCollection.Contains(((MonoBehaviour)x).gameObject)))
            {
                _gameObjectCollection.Add(((MonoBehaviour)rv).gameObject);
                GameObjectAdded?.Invoke(((MonoBehaviour)rv).gameObject);
            }
        }

        public void Remove(IRoomVisitor roomVisitor)
        {
            if(roomVisitor is not MonoBehaviour mo || !_gameObjectCollection.Contains(mo.gameObject)) return;
            
            foreach (IRoomVisitor rv in mo.gameObject.GetComponentsInChildren<IRoomVisitor>()
                         .Where(x=> _gameObjectCollection.Contains(((MonoBehaviour)x).gameObject)))
            {
                _gameObjectCollection.Remove(((MonoBehaviour)rv).gameObject);
                GameObjectRemoved?.Invoke(((MonoBehaviour)rv).gameObject);
            }
        }

        public void Enter(IRoomWalker roomWalker)
        {
            TryInitialize(_roomInitializer?.Initialize(this) ?? new GameObject[]{});
            PauseRoom(false);
        }

        public void Exit(IRoomWalker roomWalker)
        {
            PauseRoom(true);
        }

        private void TryInitialize(IEnumerable<GameObject> gameObjects)
        {
            if (!_firstTimeEnter) return;
            
            _gameObjectCollection = new SmartGameObjectCollection(gameObjects);
                
            foreach (GameObject gameObject in _gameObjectCollection)
            {
                if (gameObject.TryGetComponent(out IPausable pausable)) _toPause[gameObject] = pausable;
            }
            
            _firstTimeEnter = false;
        }
        
        private void PauseRoom(bool value)
        {
            foreach (GameObject gameObject in _gameObjectCollection)
            {
                if (_toPause.TryGetValue(gameObject, out IPausable pausable)) pausable.Paused = value;
                
                gameObject.SetActive(!value);
            }   
        }
    }
}