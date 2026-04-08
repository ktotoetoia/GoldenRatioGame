using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.LifeCycle;
using IM.Map;
using IM.Modules;
using IM.SaveSystem;
using UnityEngine;

namespace IM
{
    [CreateAssetMenu(fileName = "MapInitializer", menuName = "MapInitializer")]
    public class MapInitializer : SceneInitializer
    {
        [SerializeField] private List<ModuleEntityEntry> _moduleEntityEntries = new ();
        [SerializeField] private GameObject _roomPrefab;
        [SerializeField] private GameObject _floorPrefab;
        [SerializeField] private GameObject _roomPortPrefab;
        private IRoomFactory _roomFactory;
        
        public override void OnSceneLoaded(GameObject initializerGO, IGameObjectFactory factory)
        {
            Floor floor = factory.Create(_floorPrefab,false).GetComponent<Floor>();
            _roomFactory = new GameObjectRoomMonoFactory(factory, _roomPrefab);
            IDataGraph<IGameObjectRoom> floorGraph = new BiDirectionalDataGraph<IGameObjectRoom>();

            C(floorGraph,factory);
            C(floorGraph,factory);
            
            floor.SetFloorGraph(floorGraph);

            foreach (RoomWalkerMono roomWalker in FindObjectsByType<RoomWalkerMono>(FindObjectsSortMode.None))
            {
                roomWalker.GoTo(floorGraph.DataNodes.FirstOrDefault().Value);
            }
        }

        private void C(IDataGraph<IGameObjectRoom> graph, IGameObjectFactory factory)
        {
            IDataNode<IGameObjectRoom> node = graph.Create(_roomFactory.Create());
            
            foreach (IModuleEntity entity in _moduleEntityEntries.Select(x => new ModuleEntityFactory().Create(x,factory)))
            {
                node.Value.Add(entity.GameObject);
            }

            if (graph.DataNodes.Count() > 1)
            {
                IDataNode<IGameObjectRoom> previous = graph.DataNodes.ElementAt(graph.DataNodes.Count()-2);
                
                graph.Connect(previous,node);
                
                GameObject from = factory.Create(_roomPortPrefab, false);
                GameObject to = factory.Create(_roomPortPrefab, false);
                
                RoomPort fromPort = from.GetComponent<RoomPort>();
                RoomPort toPort = to.GetComponent<RoomPort>();
                
                from.transform.localPosition = new Vector3(4,0,0);
                to.transform.localPosition = new Vector3(-4,0,0);
                
                fromPort.Initialize(previous.Value,toPort);
                toPort.Initialize(node.Value,fromPort);

                previous.Value.Add(fromPort);
                node.Value.Add(toPort);
            }
        }
    }
}