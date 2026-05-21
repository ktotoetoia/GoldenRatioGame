using IM.Map.Grid;
using UnityEngine;

namespace IM.Map
{
    public class RoomPort : MonoBehaviour, IRoomPort
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        [Header("Sprites: None (Regular N/S)")]
        [SerializeField] private Sprite _noneClosedSpriteRegular;
        [SerializeField] private Sprite _noneOpenSpriteRegular;
        [Header("Sprites: None (Sideways E/W)")]
        [SerializeField] private Sprite _noneClosedSpriteSideways;
        [SerializeField] private Sprite _noneOpenSpriteSideways;

        [Header("Sprites: Special (Regular N/S)")]
        [SerializeField] private Sprite _specialClosedSpriteRegular;
        [SerializeField] private Sprite _specialOpenSpriteRegular;
        [Header("Sprites: Special (Sideways E/W)")]
        [SerializeField] private Sprite _specialClosedSpriteSideways;
        [SerializeField] private Sprite _specialOpenSpriteSideways;

        [Header("Sprites: Final (Regular N/S)")]
        [SerializeField] private Sprite _finalClosedSpriteRegular;
        [SerializeField] private Sprite _finalOpenSpriteRegular;
        [Header("Sprites: Final (Sideways E/W)")]
        [SerializeField] private Sprite _finalClosedSpriteSideways;
        [SerializeField] private Sprite _finalOpenSpriteSideways;

        private bool _isInitialized;
        private bool _isOpen;
        private PortSide _portSide;

        public IGameObjectRoom Origin { get; private set; }
        public IRoomPort Connection { get; private set; }
        public float NormalizedPosition { get; set; }
        public Vector2Int CellOffset { get; set; }
        
        public PortSide PortSide 
        { 
            get => _portSide; 
            set 
            {
                _portSide = value;
                UpdateVisuals();
            }
        }

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
            if (originType == RoomType.Final || destinationType == RoomType.Final) return RoomType.Final;
            if (originType == RoomType.Special || destinationType == RoomType.Special) return RoomType.Special;

            return RoomType.None;
        }

        private void UpdateVisuals()
        {
            if (!_spriteRenderer) return;

            bool isSideways = PortSide is PortSide.East or PortSide.West;
            
            _spriteRenderer.sprite = Mode switch
            {
                RoomType.Special => isSideways 
                    ? (_isOpen ? _specialOpenSpriteSideways : _specialClosedSpriteSideways) 
                    : (_isOpen ? _specialOpenSpriteRegular : _specialClosedSpriteRegular),
                    
                RoomType.Final   => isSideways 
                    ? (_isOpen ? _finalOpenSpriteSideways : _finalClosedSpriteSideways) 
                    : (_isOpen ? _finalOpenSpriteRegular : _finalClosedSpriteRegular),
                    
                _                => isSideways 
                    ? (_isOpen ? _noneOpenSpriteSideways : _noneClosedSpriteSideways) 
                    : (_isOpen ? _noneOpenSpriteRegular : _noneClosedSpriteRegular)
            };
        }

        public void Initialize(IGameObjectRoom origin)
        {
            if (_isInitialized) 
            {
                Debug.LogWarning($"Port on {gameObject.name} is already initialized");
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