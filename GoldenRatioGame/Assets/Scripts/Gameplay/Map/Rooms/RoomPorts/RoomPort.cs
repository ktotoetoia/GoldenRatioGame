using UnityEngine;

namespace IM.Map
{
    public class RoomPort : MonoBehaviour, IRoomPort
    {
        public IRoom Origin { get; private set; }
        public IRoomPort Connection { get; private set; }
    
        public Vector3 Position => transform.position;
        public bool IsConnected => Connection != null;
        private bool _isInitialized;
        public bool IsOpen { get; set; } = true;

        public void Initialize(IRoom origin, IRoomPort destination)
        {
            if (_isInitialized) 
            {
                Debug.LogWarning($"Port on {gameObject.name} is already initialized!");
                return;
            }

            Origin = origin;
            Connection = destination;
            _isInitialized = true;
        }
    }
}