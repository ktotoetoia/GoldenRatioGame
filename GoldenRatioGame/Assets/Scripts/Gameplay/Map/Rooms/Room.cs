using System.Collections.Generic;
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
        
        public GameObjectRoom(IRoomInitializer roomInitializer)
        {
            _roomInitializer = roomInitializer;
        }
        
        public void Enter()
        {
            TryInitialize();
            PauseRoom(false);
        }

        public void Exit()
        {
            PauseRoom(true);
        }

        private void TryInitialize()
        {
            if (!_firstTimeEnter) return;
            
            _gameObjectCollection = new SmartGameObjectCollection(_roomInitializer.Initialize(this));
                
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