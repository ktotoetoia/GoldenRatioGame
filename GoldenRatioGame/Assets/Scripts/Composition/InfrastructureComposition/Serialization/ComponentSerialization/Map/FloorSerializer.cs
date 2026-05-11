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
            FloorInfo floorInfo = new FloorInfo();

            foreach (IGameObjectRoom room in component.FloorGraph.DataNodes.Select(x => x.Value))
            {
                RoomInfo roomInfo = new RoomInfo
                {
                    Rect = room.Rect,
                    RoomId = GetId(room),
                    GameObjects = room.GameObjects
                        .Select(x => x.GetComponent<IIdentifiable>()?.Id)
                        .Where(id => id != null)
                        .ToList(),
                };

                foreach (IRoomPort port in room.RoomPorts)
                {
                    if (port is not MonoBehaviour portBehaviour ||
                        !portBehaviour.TryGetComponent(out IIdentifiable portId))
                        continue;

                    RoomPortInfo portInfo = new()
                    {
                        PortId = portId.Id
                    };

                    if (port.IsConnected &&
                        port.Origin is MonoBehaviour origin &&
                        origin.TryGetComponent(out IIdentifiable originId) &&
                        port.Connection is MonoBehaviour connection &&
                        connection.TryGetComponent(out IIdentifiable connectionId))
                    {
                        portInfo.Origin = originId.Id;
                        portInfo.Destination = connectionId.Id;
                        portInfo.PortSide = (int)port.PortSide;
                        portInfo.NormalizedPosition = port.NormalizedPosition;
                    }

                    roomInfo.RoomPorts.Add(portInfo);
                }

                floorInfo.RoomInfos.Add(roomInfo);
            }

            foreach (IDataEdge<IGameObjectRoom> edge in component.FloorGraph.DataEdges)
            {
                floorInfo.Connections.Add(new Connection
                {
                    From = GetId(edge.DataNode1.Value),
                    To = GetId(edge.DataNode2.Value)
                });
            }

            return floorInfo;
        }

        public override void RestoreState(Floor component, object state, Func<string, GameObject> resolveDependency)
        {
            if (state is not FloorInfo info) return;

            BiDirectionalDataGraph<IGameObjectRoom> graph = new();
            Dictionary<string, IDataNode<IGameObjectRoom>> nodeLookup = new();

            foreach (Connection conn in info.Connections)
            {
                IDataNode<IGameObjectRoom> GetOrCreateNode(string id)
                {
                    if (nodeLookup.TryGetValue(id, out IDataNode<IGameObjectRoom> node))
                        return node;

                    IGameObjectRoom room = resolveDependency(id).GetComponent<IGameObjectRoom>();

                    return nodeLookup[id] = graph.Create(room);
                }

                graph.Connect(GetOrCreateNode(conn.From), GetOrCreateNode(conn.To));
            }

            component.SetFloorGraph(graph);

            foreach (RoomInfo roomInfo in info.RoomInfos)
            {
                IGameObjectRoom room = resolveDependency(roomInfo.RoomId)
                    .GetComponent<IGameObjectRoom>();

                room.SetRect(roomInfo.Rect);

                // Restore ports BEFORE adding them to room
                foreach (RoomPortInfo portInfo in roomInfo.RoomPorts)
                {
                    RoomPort port = resolveDependency(portInfo.PortId)
                        .GetComponent<RoomPort>();

                    port.PortSide = (PortSide)portInfo.PortSide;
                    port.NormalizedPosition = portInfo.NormalizedPosition;

                    if (!string.IsNullOrEmpty(portInfo.Origin))
                    {
                        IGameObjectRoom origin = resolveDependency(portInfo.Origin)
                            .GetComponent<IGameObjectRoom>();

                        port.Initialize(origin);
                    }

                    if (!string.IsNullOrEmpty(portInfo.Destination))
                    {
                        IRoomPort destination = resolveDependency(portInfo.Destination)
                            .GetComponent<IRoomPort>();

                        port.SetDestination(destination);
                    }

                    room.Add(port);
                }

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
            }
        }

        [Serializable]
        private class FloorInfo
        {
            public List<RoomInfo> RoomInfos = new();
            public List<Connection> Connections = new();
        }

        [Serializable]
        private class Connection
        {
            public string From, To;
        }

        [Serializable]
        private class RoomInfo
        {
            public string RoomId;
            public Rect Rect;
            public List<string> GameObjects = new();
            public List<RoomPortInfo> RoomPorts = new();
        }

        [Serializable]
        private class RoomPortInfo
        {
            public string PortId;
            public string Origin;
            public string Destination;
            public int PortSide;
            public float NormalizedPosition;
        }
    }
}