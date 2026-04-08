using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IM.Graphs;
using IM.LifeCycle;
using IM.Map;
using IM.SaveSystem;

namespace IM
{
    public class FloorSerializer : ComponentSerializer<Floor>
    {
        private static string GetId(object obj) => (obj as MonoBehaviour)?.GetComponent<IIdentifiable>()?.Id;

        public override object CaptureState(Floor component)
        {
            FloorInfo info = new FloorInfo();

            foreach (IGameObjectRoom node in component.FloorGraph.DataNodes.Select(x => x.Value))
            {
                info.RoomInfos.Add(new RoomInfo
                {
                    RoomId = GetId(node),
                    GameObjects = node.GameObjects.Select(x => x.GetComponent<IIdentifiable>().Id).Where(id => id != null).ToList(),
                    RoomPorts = node.RoomPorts.Select(GetId).Where(id => id != null).ToList(),
                });
            }

            foreach (IDataEdge<IGameObjectRoom> edge in component.FloorGraph.DataEdges)
            {
                info.Connections.Add(new Connection { From = GetId(edge.DataNode1.Value), To = GetId(edge.DataNode2.Value) });
            }

            return info;
        }

        public override void RestoreState(Floor component, object state, Func<string, GameObject> resolveDependency)
        {
            if (state is not FloorInfo info) return;

            BiDirectionalDataGraph<IGameObjectRoom> graph = new BiDirectionalDataGraph<IGameObjectRoom>();
            Dictionary<string, IDataNode<IGameObjectRoom>> nodeLookup = new Dictionary<string, IDataNode<IGameObjectRoom>>();

            foreach (Connection conn in info.Connections)
            {
                IDataNode<IGameObjectRoom> GetOrCreateNode(string id)
                {
                    if (nodeLookup.TryGetValue(id, out IDataNode<IGameObjectRoom> node)) return node;
                    IGameObjectRoom room = resolveDependency(id).GetComponent<IGameObjectRoom>();
                    return nodeLookup[id] = graph.Create(room);
                }

                graph.Connect(GetOrCreateNode(conn.From), GetOrCreateNode(conn.To));
            }

            component.SetFloorGraph(graph);

            foreach (RoomInfo roomInfo in info.RoomInfos)
            {
                IGameObjectRoom room = resolveDependency(roomInfo.RoomId).GetComponent<IGameObjectRoom>();

                foreach (string id in roomInfo.GameObjects)
                {
                    GameObject go = resolveDependency(id);

                    if (go.TryGetComponent(out IRoomWalker roomWalker))
                    {
                        roomWalker.GoTo(room);
                        
                        continue;
                    }
                    
                    room.Add(go);
                }
                foreach (string id in roomInfo.RoomPorts) room.Add(resolveDependency(id).GetComponent<IRoomPort>());
            }
        }

        [Serializable]
        private class FloorInfo
        {
            public List<RoomInfo> RoomInfos = new();
            public List<Connection> Connections = new();
        }

        [Serializable]
        private class Connection { public string From, To; }

        [Serializable]
        private class RoomInfo
        {
            public string RoomId;
            public List<string> GameObjects;
            public List<string> RoomPorts;
        }
    }
}