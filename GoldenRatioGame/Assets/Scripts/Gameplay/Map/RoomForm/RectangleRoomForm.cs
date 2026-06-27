using UnityEngine;

namespace IM.Map
{
    [DefaultExecutionOrder(-100)]
    [RequireComponent(typeof(GameObjectRoomMono))]
    public class RectangleRoomForm : MonoBehaviour, IRoomForm
    {
        [SerializeField] private int _portTileRadius = 1;

        private const int WallBorder = 1;

        private GameObjectRoomMono _gameObjectRoomMono;
        private IRoomFormLayer[] _layers;

        public IRoomShape RoomShape { get; private set; }

        private void Awake()
        {
            _gameObjectRoomMono = GetComponent<GameObjectRoomMono>();
            _layers = GetComponents<IRoomFormLayer>();
            _gameObjectRoomMono.RoomPortAdded += SetupRoomPort;
        }

        private void OnDestroy()
        {
            if (_gameObjectRoomMono != null)
                _gameObjectRoomMono.RoomPortAdded -= SetupRoomPort;
        }

        public void Apply(IRoomShape shape)
        {
            if (shape == null) return;

            RoomShape = shape;

            foreach (var layer in _layers)
                layer.Apply(shape);

            if (_gameObjectRoomMono?.RoomPorts == null) return;
            foreach (IRoomPort port in _gameObjectRoomMono.RoomPorts)
                SetupRoomPort(port);
        }

        private void SetupRoomPort(IRoomPort port)
        {
            if (port is not MonoBehaviour mb || RoomShape == null || !RoomShape.Metrics.IsValid)
                return;

            var (tilePos, isHorizontal) = ResolvePortTile(port.PortIdentity);
            mb.transform.localPosition = new Vector3(tilePos.x + 0.5f, tilePos.y + 0.5f, 0f);

            var portInfo = new PortTileInfo(tilePos, isHorizontal, _portTileRadius);
            foreach (var layer in _layers)
                layer.SetupPort(portInfo);
        }

        private (Vector3Int tilePos, bool isHorizontal) ResolvePortTile(IPortIdentity identity)
        {
            var (cellStartX, cellStartY) = RoomShape.Metrics.GetCellOrigin(identity.CellOffset);

            int wallLeft   = cellStartX - WallBorder;
            int wallRight  = cellStartX + RoomShape.Metrics.CellW - 1 + WallBorder;
            int wallBottom = cellStartY - WallBorder;
            int wallTop    = cellStartY + RoomShape.Metrics.CellH - 1 + WallBorder;

            float norm = identity.NormalizedPosition;
            bool isHorizontal = identity.PortSide is PortSide.North or PortSide.South;

            Vector3Int tilePos = identity.PortSide switch
            {
                PortSide.North => new(Mathf.RoundToInt(Mathf.Lerp(wallLeft, wallRight, norm)), wallTop,    0),
                PortSide.South => new(Mathf.RoundToInt(Mathf.Lerp(wallLeft, wallRight, norm)), wallBottom, 0),
                PortSide.East  => new(wallRight, Mathf.RoundToInt(Mathf.Lerp(wallBottom, wallTop, norm)),  0),
                PortSide.West  => new(wallLeft,  Mathf.RoundToInt(Mathf.Lerp(wallBottom, wallTop, norm)),  0),
                _              => Vector3Int.zero
            };

            return (tilePos, isHorizontal);
        }
    }
}