using IM.LifeCycle;
using UnityEngine;

namespace IM
{
    public class PauseManager : MonoBehaviour, IPauseManager, IGameObjectFactoryObserver
    {
        private readonly SmartCollection<IPausable> _toPause = new();
        
        public bool Paused { get; private set; }
        
        public void OnCreate(GameObject instance, bool deserialized)
        {
            IPausable[] toPause = instance.GetComponentsInChildren<IPausable>();
            
            foreach (IPausable pausable in toPause) 
                _toPause.Add(pausable);
        }
        
        public void SetPaused(bool paused)
        {
            if(Paused == paused) return;
            
            Paused = paused;
            
            Time.timeScale = Paused ? 0 : 1;
            
            foreach (IPausable pausable in _toPause) 
                pausable.Paused = paused;
        }
    }
}