using System;
using UnityEngine;

namespace IM.Map
{
    public class RoomPort : MonoBehaviour, IRoomPort
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private RoomTypePortSprites _noneTypeSprites;
        [SerializeField] private RoomTypePortSprites _specialTypeSprites;
        [SerializeField] private RoomTypePortSprites _finalTypeSprites;
        [SerializeField] private GameObject _northOpen;
        [SerializeField] private GameObject _southOpen;
        [SerializeField] private GameObject _eastOpen;
        [SerializeField] private GameObject _westOpen;
        [SerializeField] private GameObject _northClosed;
        [SerializeField] private GameObject _southClosed;
        [SerializeField] private GameObject _eastClosed;
        [SerializeField] private GameObject _westClosed;
        
        private BoxCollider2D _localBoxCollider;
        private bool _isInitialized;
        private bool _isOpen;
        
        public IGameObjectRoom Origin { get; private set; }
        public IRoomPort Destination { get; private set; }
        public IPortIdentity PortIdentity { get; private set; }
        public Vector3 EnterPosition => transform.position;
        public Vector3 DeploymentPosition => transform.position + 
                                             new Vector3(PortSideUtility.ToDirection(PortSideUtility.Opposite(PortIdentity.PortSide)).x, 
                                                         PortSideUtility.ToDirection(PortSideUtility.Opposite(PortIdentity.PortSide)).y) * 1.5f;
        public bool IsConnected => Destination != null;
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

        private void Awake()
        {
            _localBoxCollider = GetComponent<BoxCollider2D>();
            if (!_localBoxCollider)
            {
                Debug.LogWarning($"BoxCollider2D missing on {gameObject.name}. Adding one dynamically.");
                _localBoxCollider = gameObject.AddComponent<BoxCollider2D>();
            }

            DeactivateAllInstances();
        }

        private void UpdateVisuals()
        {
            UpdateActiveSideInstance();

            if (!_spriteRenderer || PortIdentity == null) return;

            RoomTypePortSprites activeTypeConfig = Mode switch
            {
                RoomType.Special => _specialTypeSprites,
                RoomType.Final   => _finalTypeSprites,
                _                => _noneTypeSprites
            };

            PortSpriteSet activeSpriteSet = activeTypeConfig.GetSetByDirection(PortIdentity.PortSide);
            _spriteRenderer.sprite = activeSpriteSet.GetSprite(_isOpen);
        }

        private void UpdateActiveSideInstance()
        {
            if (PortIdentity == null) return;

            DeactivateAllInstances();

            GameObject targetInstance = PortIdentity.PortSide switch
            {
                PortSide.North => _isOpen ? _northOpen : _northClosed,
                PortSide.South => _isOpen ? _southOpen : _southClosed,
                PortSide.East  => _isOpen ? _eastOpen : _eastClosed,
                PortSide.West  => _isOpen ? _westOpen : _westClosed,
                _ => null
            };

            if (targetInstance)
            {
                targetInstance.SetActive(true);

                Transform collidersChild = targetInstance.transform.Find("Colliders");
                
                if (collidersChild) collidersChild.gameObject.SetActive(true);

                if (targetInstance.TryGetComponent<BoxCollider2D>(out var sourceCollider))
                {
                    _localBoxCollider.size = sourceCollider.size;
                    _localBoxCollider.offset = sourceCollider.offset;
                    _localBoxCollider.isTrigger = sourceCollider.isTrigger;
                }
            }
        }

        private void DeactivateAllInstances()
        {
            GameObject[] allInstances = 
            {
                _northOpen, _northClosed, 
                _southOpen, _southClosed, 
                _eastOpen, _eastClosed, 
                _westOpen, _westClosed
            };

            foreach (var instance in allInstances)
            {
                if (!instance) continue;
                
                instance.SetActive(false);
                
                Transform collidersChild = instance.transform.Find("Colliders");
                
                if (collidersChild) collidersChild.gameObject.SetActive(false);
            }
        }

        private RoomType DetermineMode()
        {
            RoomType originType = (Origin as IHaveRoomType)?.RoomType ?? RoomType.None;

            if (Destination == null) return originType;

            RoomType destinationType = (Destination.Origin as IHaveRoomType)?.RoomType ?? RoomType.None;
            if (originType == RoomType.Final || destinationType == RoomType.Final) return RoomType.Final;
            if (originType == RoomType.Special || destinationType == RoomType.Special) return RoomType.Special;
            
            return RoomType.None;
        }

        public void SetDestination(IRoomPort destination)
        {
            Destination = destination;
            UpdateVisuals();
        }

        public void Initialize(IGameObjectRoom origin, IPortIdentity pi)
        {
            if (_isInitialized) 
            {
                Debug.LogWarning($"Port on {gameObject.name} is already initialized");
                return;
            }

            Origin = origin;
            PortIdentity = new PortIdentity(pi.Index, pi.NormalizedPosition, pi.CellOffset, pi.PortSide);
            _isInitialized = true;
            
            UpdateVisuals();
        }
        
        [Serializable]
        public struct PortSpriteSet
        {
            public Sprite OpenSprite;
            public Sprite ClosedSprite;

            public Sprite GetSprite(bool isOpen) => isOpen ? OpenSprite : ClosedSprite;
        }

        [Serializable]
        public class RoomTypePortSprites
        {
            public PortSpriteSet North;
            public PortSpriteSet South;
            public PortSpriteSet East;
            public PortSpriteSet West;

            public PortSpriteSet GetSetByDirection(PortSide side)
            {
                return side switch
                {
                    PortSide.North => North,
                    PortSide.South => South,
                    PortSide.East  => East,
                    PortSide.West  => West,
                    _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
                };
            }
        }
    }
}