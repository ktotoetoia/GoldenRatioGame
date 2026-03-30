using System.Linq;
using IM.Graphs;
using IM.LifeCycle;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IM.Map
{
    public class TiledRoomTest : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _fillTile;
        [SerializeField] private TileBase _borderTile;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private GameObjectFactory _gameObjectFactory;
        private IFloor _floor;
        private IRoomFactory _roomFactory;
        private IRoomWalker _roomWalker;
        
        private void Awake()
        {
            _roomFactory = new GameObjectRoomMonoFactory(_gameObjectFactory, _prefab);
            IDataGraph<IRoom> floorGraph = new BiDirectionalDataGraph<IRoom>();
            IDataNode<IRoom> root = floorGraph.Create(_roomFactory.Create());
            C(floorGraph);
            C(floorGraph);
            C(floorGraph);
            C(floorGraph);

            _roomWalker = new RoomWalker(root);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                _roomWalker.GoTo(_roomWalker.Available.FirstOrDefault());
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                _roomWalker.GoTo(_roomWalker.Available.LastOrDefault());
            }
        }

        private void C(IDataGraph<IRoom> graph)
        {
            IDataNode<IRoom> node = graph.Create(_roomFactory.Create());
            
            graph.Connect(graph.DataNodes.ElementAt(graph.DataNodes.Count()-2),node);
        }
    }
}