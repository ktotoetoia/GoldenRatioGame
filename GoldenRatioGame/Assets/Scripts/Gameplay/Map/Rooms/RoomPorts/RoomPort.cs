using IM.Map.Grid;
using UnityEngine;

namespace IM.Map
{
    public class RoomPort : MonoBehaviour, IRoomPort
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        [Header("Sprites: None")]
        [SerializeField] private Sprite _noneClosedSprite;
        [SerializeField] private Sprite _noneOpenSprite;
        
        [Header("Sprites: Special")]
        [SerializeField] private Sprite _specialClosedSprite;
        [SerializeField] private Sprite _specialOpenSprite;
        
        [Header("Sprites: Final")]
        [SerializeField] private Sprite _finalClosedSprite;
        [SerializeField] private Sprite _finalOpenSprite;

        private bool _isInitialized;
        private bool _isOpen;

        public IGameObjectRoom Origin { get; private set; }
        public IRoomPort Connection { get; private set; }
        public float NormalizedPosition { get; set; }
        public PortSide PortSide { get; set; }

        public Vector3 EnterPosition => transform.position;
        public Vector3 DeploymentPosition => transform.position + 
                                             new Vector3(PortSideUtility.ToDirection(PortSideUtility.Opposite(PortSide)).x, 
                                                         PortSideUtility.ToDirection(PortSideUtility.Opposite(PortSide)).y);

        public bool IsConnected => Connection != null;

        public RoomType Mode => DetermineMode();

        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                _isOpen = value;
                UpdateVisuals();
            }
        }

        private RoomType DetermineMode()
        {
            RoomType originType = (Origin as IHaveRoomType)?.RoomType ?? RoomType.None;

            if (Connection == null) return originType;

            RoomType destinationType = (Connection.Origin as IHaveRoomType)?.RoomType ?? RoomType.None;

            if (originType == RoomType.Final || destinationType == RoomType.Final)
                return RoomType.Final;
            
            if (originType == RoomType.Special || destinationType == RoomType.Special)
                return RoomType.Special;

            return RoomType.None;
        }

        private void UpdateVisuals()
        {
            if (!_spriteRenderer) return;
            
            _spriteRenderer.sprite = Mode switch
            {
                RoomType.Special => _isOpen ? _specialOpenSprite : _specialClosedSprite,
                RoomType.Final   => _isOpen ? _finalOpenSprite   : _finalClosedSprite,
                _                => _isOpen ? _noneOpenSprite    : _noneClosedSprite
            };
        }

        public void Initialize(IGameObjectRoom origin)
        {
            if (_isInitialized) 
            {
                Debug.LogWarning($"Port on {gameObject.name} is already initialized!");
                return;
            }

            Origin = origin;
            _isInitialized = true;
            UpdateVisuals();
        }

        public void SetDestination(IRoomPort destination)
        {
            Connection = destination;
            UpdateVisuals();
        }
    }
}