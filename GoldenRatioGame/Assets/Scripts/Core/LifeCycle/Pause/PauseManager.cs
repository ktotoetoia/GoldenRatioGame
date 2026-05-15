using UnityEngine;

namespace IM.LifeCycle
{
    public class PauseManager : MonoBehaviour, IPauseManager, IGameObjectFactoryObserver
    {
        [SerializeField] private bool _startPaused;
        private readonly SmartCollection<IPausable> _toPause = new();
        
        public bool Paused { get; private set; }

        private void Awake()
        {
            SetPausedInternal(_startPaused);
        }
        
        public void OnCreate(GameObject instance, bool deserialized)
        {
            IPausable[] toPause = instance.GetComponentsInChildren<IPausable>();
            
            foreach (IPausable pausable in toPause) 
                _toPause.Add(pausable);
        }
        
        public void SetPaused(bool paused)
        {
            if(Paused == paused) return;
            
            SetPausedInternal(paused);
        }

        private void SetPausedInternal(bool paused)
        {
            Paused = paused;
            
            Time.timeScale = Paused ? 0 : 1;
            
            foreach (IPausable pausable in _toPause) 
                pausable.Paused = paused;
        }
    }
}