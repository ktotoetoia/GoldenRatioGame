using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IM.Graphs;
using IM.LifeCycle;
using IM.Map;
using IM.Map.Grid;
using IM.SaveSystem;
using UnityEngine.AddressableAssets;

namespace IM
{
    public class FloorSerializer : ComponentSerializer<Floor>
    {
        private static string GetId(object obj) => (obj as MonoBehaviour)?.GetComponent<IIdentifiable>()?.Id;

        private static string GetRoomId(IRoomWalker walker) => GetId(walker.Current);

        public override object CaptureState(Floor component)
        {
            FloorInfo floorInfo = new()
            {
                MapInfoAddress = component.MapInfoFactory.AddresableAddress,
                Seed = component.Seed,
                Depth = component.Depth
            };

            foreach (IRoomWalker roomWalker in component.RoomWalkers)
            {
                if (roomWalker is not MonoBehaviour mb)
                {
                    Debug.LogWarning("Floor has a room walker that is not MonoBehaviour, cannot serialize");
                    continue;
                }

                if (!mb.TryGetComponent(out IIdentifiable identifiable))
                {
                    Debug.LogWarning("Room Walker object does not contain component IIdentifiable");
                    continue;
                }

                floorInfo.RoomWalkers.Add(new RoomWalkerInfo
                {
                    WalkerId = identifiable.Id,
                    RoomId = GetRoomId(roomWalker)
                });
            }

            foreach (IGameObjectRoom room in component.FloorGraph.DataNodes.Select(x => x.Value))
            {
                RoomInfo roomInfo = new()
                {
                    RoomId = GetId(room),
                    GameObjects = room.GameObjects
                        .Select(x => x.GetComponent<IIdentifiable>()?.Id)
                        .Where(id => id != null)
                        .ToList()
                };

                foreach (IRoomPort port in room.RoomPorts)
                {
                    if (port is not MonoBehaviour portBehaviour ||
                        !portBehaviour.TryGetComponent(out IIdentifiable portId))
                        continue;

                    RoomPortInfo portInfo = new() { PortId = portId.Id };

                    if (port.IsConnected &&
                        port.Origin is MonoBehaviour origin &&
                        origin.TryGetComponent(out IIdentifiable originId) &&
                        port.Connection is MonoBehaviour connection &&
                        connection.TryGetComponent(out IIdentifiable connectionId))
                    {
                        portInfo.Origin = originId.Id;
                        portInfo.Destination = connectionId.Id;
                        portInfo.PortIdentity = new PortIdentityInfo
                        {
                            NormalizedPosition = port.PortIdentity.NormalizedPosition,
                            PortSide = (int)port.PortIdentity.PortSide,
                            Index = port.PortIdentity.Index,
                            CellOffset = port.PortIdentity.CellOffset
                        };
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

            var operation = Addressables.LoadAssetAsync<IMapInfoFactory>(info.MapInfoAddress);
            component.SetMapFactory(operation.WaitForCompletion());
            component.Seed = info.Seed;
            component.Depth = info.Depth;

            BiDirectionalDataGraph<IGameObjectRoom> graph = new();
            Dictionary<string, IDataNode<IGameObjectRoom>> nodeLookup = new();

            IDataNode<IGameObjectRoom> GetOrCreateNode(string id)
            {
                if (nodeLookup.TryGetValue(id, out var node)) return node;
                IGameObjectRoom room = resolveDependency(id).GetComponent<IGameObjectRoom>();
                return nodeLookup[id] = graph.Create(room);
            }

            foreach (Connection conn in info.Connections)
                graph.Connect(GetOrCreateNode(conn.From), GetOrCreateNode(conn.To));

            component.Next(new MapInfo(graph));

            foreach (RoomInfo roomInfo in info.RoomInfos)
            {
                IGameObjectRoom room = resolveDependency(roomInfo.RoomId).GetComponent<IGameObjectRoom>();

                foreach (RoomPortInfo portInfo in roomInfo.RoomPorts)
                {
                    RoomPort port = resolveDependency(portInfo.PortId).GetComponent<RoomPort>();

                    PortIdentity portIdentity = new(
                        portInfo.PortIdentity.Index,
                        portInfo.PortIdentity.NormalizedPosition,
                        portInfo.PortIdentity.CellOffset,
                        (PortSide)portInfo.PortIdentity.PortSide);

                    IGameObjectRoom origin = string.IsNullOrEmpty(portInfo.Origin)
                        ? null
                        : resolveDependency(portInfo.Origin).GetComponent<IGameObjectRoom>();

                    port.Initialize(origin, portIdentity);

                    if (!string.IsNullOrEmpty(portInfo.Destination))
                    {
                        IRoomPort destination = resolveDependency(portInfo.Destination).GetComponent<IRoomPort>();
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

            foreach (RoomWalkerInfo walkerInfo in info.RoomWalkers)
            {
                GameObject go = resolveDependency(walkerInfo.WalkerId);
                if (!go || !go.TryGetComponent(out IRoomWalker roomWalker)) continue;

                IGameObjectRoom room = string.IsNullOrEmpty(walkerInfo.RoomId)
                    ? null
                    : resolveDependency(walkerInfo.RoomId).GetComponent<IGameObjectRoom>();

                component.AddRoomWalker(roomWalker, room);
            }
        }

        [Serializable]
        private class FloorInfo
        {
            public int Seed;
            public int Depth;
            public string MapInfoAddress;
            public List<RoomWalkerInfo> RoomWalkers = new();
            public List<RoomInfo> RoomInfos = new();
            public List<Connection> Connections = new();
        }

        [Serializable]
        private class RoomWalkerInfo
        {
            public string WalkerId;
            public string RoomId;
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
            public List<string> GameObjects = new();
            public List<RoomPortInfo> RoomPorts = new();
        }

        [Serializable]
        private class RoomPortInfo
        {
            public string PortId;
            public string Origin;
            public string Destination;
            public PortIdentityInfo PortIdentity;
        }

        [Serializable]
        private struct PortIdentityInfo
        {
            public Vector2Int CellOffset;
            public float NormalizedPosition;
            public int Index;
            public int PortSide;
        }
    }
}