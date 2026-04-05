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
        private string GetId(object obj) => (obj as MonoBehaviour)?.GetComponent<IIdentifiable>()?.Id;

        public override object CaptureState(Floor component)
        {
            FloorInfo info = new FloorInfo();

            foreach (IRoom node in component.FloorGraph.DataNodes.Select(x => x.Value))
            {
                info.RoomInfos.Add(new RoomInfo
                {
                    RoomId = GetId(node),
                    RoomVisitors = node.RoomVisitors.Select(GetId).Where(id => id != null).ToList(),
                    RoomPorts = node.RoomPorts.Select(GetId).Where(id => id != null).ToList(),
                });
            }

            foreach (IDataEdge<IRoom> edge in component.FloorGraph.DataEdges)
            {
                info.Connections.Add(new Connection { From = GetId(edge.DataNode1.Value), To = GetId(edge.DataNode2.Value) });
            }

            return info;
        }

        public override void RestoreState(Floor component, object state, Func<string, GameObject> resolveDependency)
        {
            if (state is not FloorInfo info) return;

            BiDirectionalDataGraph<IRoom> graph = new BiDirectionalDataGraph<IRoom>();
            Dictionary<string, IDataNode<IRoom>> nodeLookup = new Dictionary<string, IDataNode<IRoom>>();

            foreach (Connection conn in info.Connections)
            {
                IDataNode<IRoom> GetOrCreateNode(string id)
                {
                    if (nodeLookup.TryGetValue(id, out IDataNode<IRoom> node)) return node;
                    IRoom room = resolveDependency(id).GetComponent<IRoom>();
                    return nodeLookup[id] = graph.Create(room);
                }

                graph.Connect(GetOrCreateNode(conn.From), GetOrCreateNode(conn.To));
            }

            component.SetFloorGraph(graph);

            foreach (RoomInfo roomInfo in info.RoomInfos)
            {
                IRoom room = resolveDependency(roomInfo.RoomId).GetComponent<IRoom>();

                foreach (string id in roomInfo.RoomVisitors)
                {
                    GameObject go = resolveDependency(id);

                    if (go.TryGetComponent(out IRoomWalker roomWalker))
                    {
                        roomWalker.GoTo(room);
                        
                        continue;
                    }
                    
                    room.Add(go.GetComponent<IRoomVisitor>());
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
            public List<string> RoomVisitors;
            public List<string> RoomPorts;
        }
    }
}