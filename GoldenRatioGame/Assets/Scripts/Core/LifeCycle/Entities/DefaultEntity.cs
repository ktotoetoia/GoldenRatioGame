using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.LifeCycle
{
    public class DefaultEntity : MonoBehaviour, IEntity, IPausable
    {
        protected bool _paused;
        protected IEnumerable<IPausable> _pausableList;
        
        public event Action<IEntity> Destroyed;

        public GameObject GameObject => gameObject;

        public bool Paused
        {
            get => _paused;
            set
            {
                if(value ==_paused) return;
                
                _pausableList ??= GetComponents<IPausable>();
                
                _paused = value;
                
                PausedChanged();
                
                foreach (IPausable pausable in _pausableList)
                {
                    pausable.Paused = value;
                }
            }
        }

        protected virtual void PausedChanged()
        {
            
        }
        
        public virtual void Destroy()
        {
            Destroy(gameObject);
        }
        
        private void OnDestroy()
        {
            Destroyed?.Invoke(this);
            Destroyed = null;
        }
    }
}