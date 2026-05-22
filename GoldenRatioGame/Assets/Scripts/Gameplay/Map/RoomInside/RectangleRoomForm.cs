using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IM.Map
{
    [DefaultExecutionOrder(-100)]
    [RequireComponent(typeof(GameObjectRoomMono))]
    public class RectangleRoomForm : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _floorTile;
        [SerializeField] private TileBase _wallTile;
        private GameObjectRoomMono _gameObjectRoomMono;
        private const int WallBorder = 1;
        
        public Rect Rect{ get; private set; }
        
        private void Awake()
        {
            _gameObjectRoomMono = GetComponent<GameObjectRoomMono>();
            _gameObjectRoomMono.RoomPortAdded += SetUpRoomPort;
        }
        
        public void SetRect(Rect rect)
        {
            Rect = rect;
            GenerateRoom((int)rect.size.x, (int)rect.size.y);
            
            if(!_gameObjectRoomMono) return;

            foreach (IRoomPort port in _gameObjectRoomMono.RoomPorts) SetUpRoomPort(port);
        }

        private void GenerateRoom(int xM, int yM)
        {
            if (!_tilemap || !_floorTile  || !_wallTile) return;

            _tilemap.ClearAllTiles();

            int startX = -xM / 2;
            int startY = -yM / 2;
            
            for (int x = -WallBorder; x < xM + WallBorder; x++)
            {
                for (int y = -WallBorder; y < yM + WallBorder; y++)
                {
                    Vector3Int pos = new Vector3Int(startX + x, startY + y, 0);
                    _tilemap.SetTile(pos, _wallTile);
                }
            }
            
            for (int x = 0; x <xM; x++)
            {
                for (int y = 0; y < yM; y++)
                {
                    Vector3Int pos = new Vector3Int(startX + x, startY + y, 0);
                    _tilemap.SetTile(pos, _floorTile);
                }
            }
        }

        private void SetUpRoomPort(IRoomPort roomPort)
        {
            if (roomPort is not MonoBehaviour mb) return;

            ResolvePortPosition(mb,Rect, roomPort.PortSide, roomPort.NormalizedPosition);
        }

        private void ResolvePortPosition(MonoBehaviour mb, Rect rect, PortSide side, float normalizedPosition)
        {
            int xM = (int)rect.width;
            int yM = (int)rect.height;
            int startX = -xM / 2;
            int startY = -yM / 2;

            int wallLeft   = startX - WallBorder;
            int wallRight  = startX + xM - 1 + WallBorder;
            int wallBottom = startY - WallBorder;
            int wallTop    = startY + yM - 1 + WallBorder;

            Vector3Int tilePos = side switch
            {
                PortSide.North => new Vector3Int(
                    Mathf.RoundToInt(Mathf.Lerp(wallLeft, wallRight, normalizedPosition)),
                    wallTop, 0),
                PortSide.South => new Vector3Int(
                    Mathf.RoundToInt(Mathf.Lerp(wallLeft, wallRight, normalizedPosition)),
                    wallBottom, 0),
                PortSide.East  => new Vector3Int(
                    wallRight,
                    Mathf.RoundToInt(Mathf.Lerp(wallBottom, wallTop, normalizedPosition)), 0),
                PortSide.West  => new Vector3Int(
                    wallLeft,
                    Mathf.RoundToInt(Mathf.Lerp(wallBottom, wallTop, normalizedPosition)), 0),
                _ => Vector3Int.zero
            };

            mb.transform.localPosition = new Vector3(tilePos.x + 0.5f, tilePos.y + 0.5f, 0f);
            
            _tilemap.SetColor(tilePos, Color.clear);
            mb.transform.localScale = side switch
            {
                PortSide.North => new Vector3(1, 1, 1),
                PortSide.South => new Vector3(1, 1, -1),
                PortSide.East => new Vector3(-1, 1, 1),
                PortSide.West => new Vector3(1, 1, 1),
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };
        }
    }
}