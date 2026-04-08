using UnityEngine;

namespace IM.Map
{
    public class RoomPort : MonoBehaviour, IRoomPort
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _closedSprite;
        [SerializeField] private Sprite _openSprite;
        private bool _isInitialized;
        private bool _isOpen;
            
        public IGameObjectRoom Origin { get; private set; }
        public IRoomPort Connection { get; private set; }
    
        public Vector3 EnterPosition => transform.position;
        public Vector3 DeploymentPosition => transform.position + Offset;
        [field: SerializeField] public Vector3 Offset { get; set; }
        public bool IsConnected => Connection != null;
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                _isOpen = value;
                if (_isOpen)
                {
                    _spriteRenderer.sprite = _openSprite;
                    
                    return;
                }
                
                _spriteRenderer.sprite = _closedSprite;
            } 
        }

        public void Initialize(IGameObjectRoom origin, IRoomPort destination)
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