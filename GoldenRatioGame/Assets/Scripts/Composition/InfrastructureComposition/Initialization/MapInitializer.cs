using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.LifeCycle;
using IM.Map;
using IM.Modules;
using IM.SaveSystem;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IM
{
    [CreateAssetMenu(fileName = "MapInitializer", menuName = "MapInitializer")]
    public class MapInitializer : SceneInitializer
    {
        [SerializeField] private List<ModuleEntityEntry> _moduleEntityEntries = new ();
        [SerializeField] private GameObject _roomPrefab;
        [SerializeField] private GameObject _floorPrefab;
        [SerializeField] private TileBase _tileToPlace;
        private int _instanceCount = 0;
        private IRoomFactory _roomFactory;
        
        public override void OnSceneLoaded(GameObject initializerGO, IGameObjectFactory factory)
        {
            _instanceCount = 0;
            
            Floor floor = factory.Create(_floorPrefab,false).GetComponent<Floor>();
            _roomFactory = new GameObjectRoomMonoFactory(factory, _roomPrefab);
            IDataGraph<IRoom> floorGraph = new BiDirectionalDataGraph<IRoom>();

            C(floorGraph,factory);
            C(floorGraph,factory);
            
            floor.SetFloorGraph(floorGraph);

            foreach (RoomWalkerMono roomWalker in FindObjectsByType<RoomWalkerMono>(FindObjectsSortMode.None))
            {
                roomWalker.Initialize(floorGraph.DataNodes.FirstOrDefault());
            }
        }

        private void C(IDataGraph<IRoom> graph, IGameObjectFactory factory)
        {
            IDataNode<IRoom> node = graph.Create(_roomFactory.Create());
            
            foreach (IModuleEntity entity in _moduleEntityEntries.Select(x => new ModuleEntityFactory().Create(x,factory)))
            {
                node.Value.Add(entity.GameObject.GetComponent<IRoomVisitor>());
            }
            _instanceCount++;
            
            for (int i = 0; i < _instanceCount; i++)
            {
                Vector3Int tilePosition = new Vector3Int(i, _instanceCount, 0);
                (node.Value as MonoBehaviour).GetComponent<Tilemap>().SetTile(tilePosition, _tileToPlace);
            }

            if (graph.DataNodes.Count() > 1)
            {
                graph.Connect(graph.DataNodes.ElementAt(graph.DataNodes.Count()-2),node);
            }
        }
    }
}