using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.LifeCycle;
using IM.Map;
using IM.SaveSystem;
using UnityEngine;

namespace IM
{
   [CreateAssetMenu(fileName = "MapInitializer", menuName = "Initialization/Map Initializer")]
    public class MapInitializer : SceneInitializer
    {
        [SerializeField] private List<RoomFactory> _roomFactories;
        [SerializeField] private GameObject _floorPrefab;
        [SerializeField] private GameObject _roomPortPrefab;
        [SerializeField] private float _portOffset = 4f;

        public override void OnSceneLoaded(GameObject initializerGO, IGameObjectFactory factory)
        {
            var floor = factory.Create(_floorPrefab, false).GetComponent<Floor>();
            var graph = new BiDirectionalDataGraph<IGameObjectRoom>();
            IDataNode<IGameObjectRoom> previousNode = null;

            foreach (var roomFactory in _roomFactories)
            {
                var room = roomFactory.Create(factory);
                var currentNode = graph.Create(room);

                if (previousNode != null)
                {
                    ConnectRooms(factory, graph, previousNode, currentNode);
                }
                previousNode = currentNode;
            }

            floor.SetFloorGraph(graph);
            UpdateWalkers(graph.DataNodes.FirstOrDefault()?.Value);
        }

        private void ConnectRooms(IGameObjectFactory factory, IDataGraph<IGameObjectRoom> graph, 
            IDataNode<IGameObjectRoom> from, IDataNode<IGameObjectRoom> to)
        {
            graph.Connect(from, to);

            var portAGO = factory.Create(_roomPortPrefab, false);
            var portBGO = factory.Create(_roomPortPrefab, false);

            portAGO.transform.localPosition = new Vector3(_portOffset, 0, 0);
            portBGO.transform.localPosition = new Vector3(-_portOffset, 0, 0);

            var portA = portAGO.GetComponent<RoomPort>();
            var portB = portBGO.GetComponent<RoomPort>();

            portA.Initialize(from.Value, portB);
            portB.Initialize(to.Value, portA);

            from.Value.Add(portA);
            to.Value.Add(portB);
        }

        private void UpdateWalkers(IGameObjectRoom startRoom)
        {
            if (startRoom == null) return;
            foreach (var walker in FindObjectsByType<RoomWalkerMono>(FindObjectsSortMode.None))
            {
                walker.GoTo(startRoom);
            }
        }
    }
}