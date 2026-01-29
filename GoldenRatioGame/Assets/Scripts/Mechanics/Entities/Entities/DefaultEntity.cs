using System.Collections.Generic;
using UnityEngine;

namespace IM.Entities
{
    public class DefaultEntity : MonoBehaviour, IEntity
    {
        protected bool _paused;
        protected IEnumerable<IPausable> _pausableList;

        public GameObject GameObject => gameObject;
        public bool Paused
        {
            get => _paused;
            set
            {
                _pausableList ??= GetComponents<IPausable>();
                
                if(value ==_paused) return;
                
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
    }
}