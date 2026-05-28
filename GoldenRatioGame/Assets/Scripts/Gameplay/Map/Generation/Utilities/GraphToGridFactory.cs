using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Map.Grid
{
    public class GraphToGridFactory
    {
        public IGameObjectRoom[,] Create(IDataGraph<IGameObjectRoom> graph)
        {
            if (graph == null || !graph.Nodes.Any())
                return new IGameObjectRoom[0, 0];

            var roomOrigins = new Dictionary<IGameObjectRoom, Vector2Int>();
            var queue = new Queue<IDataNode<IGameObjectRoom>>();
            var visited = new HashSet<IDataNode<IGameObjectRoom>>();

            var startNode = graph.DataNodes.First();
            queue.Enqueue(startNode);
            visited.Add(startNode);

            roomOrigins[startNode.Value] = Vector2Int.zero;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                IGameObjectRoom roomA = current.Value;
                Vector2Int originA = roomOrigins[roomA];

                var connectedEdges = graph.DataEdges.Where(e => e.Node1 == current || e.Node2 == current);

                foreach (var edge in connectedEdges)
                {
                    var neighborNode = edge.DataNode1 == current ? edge.DataNode2 : edge.DataNode1;
                    if (visited.Contains(neighborNode)) continue;

                    IGameObjectRoom roomB = neighborNode.Value;

                    if (TryCalculateNeighborOrigin(roomA, roomB, originA, out Vector2Int originB))
                    {
                        roomOrigins[roomB] = originB;
                        visited.Add(neighborNode);
                        queue.Enqueue(neighborNode);
                    }
                }
            }

            return Build2DArray(roomOrigins);
        }

        private bool TryCalculateNeighborOrigin(
            IGameObjectRoom roomA, 
            IGameObjectRoom roomB, 
            Vector2Int originA, 
            out Vector2Int originB)
        {
            originB = Vector2Int.zero;

            foreach (IRoomPort portA in roomA.RoomPorts)
            {
                IRoomPort portB = portA.Destination; 

                if (portB != null && roomB.RoomPorts.Contains(portB))
                {
                    Vector2Int offsetA = portA.PortIdentity.CellOffset;
                    Vector2Int offsetB = portB.PortIdentity.CellOffset;

                    Vector2Int direction = PortSideUtility.ToDirection(portA.PortIdentity.PortSide);

                    originB = originA + offsetA + direction - offsetB;
                    return true;
                }
            }

            return false;
        }

        private IGameObjectRoom[,] Build2DArray(Dictionary<IGameObjectRoom, Vector2Int> roomOrigins)
        {
            var cellMap = new Dictionary<Vector2Int, IGameObjectRoom>();
            int minX = int.MaxValue, minY = int.MaxValue;
            int maxX = int.MinValue, maxY = int.MinValue;

            foreach (var kvp in roomOrigins)
            {
                IGameObjectRoom room = kvp.Key;
                Vector2Int origin = kvp.Value;
                IRoomForm roomForm = ((MonoBehaviour)room).GetComponent<IRoomForm>();

                IEnumerable<Vector2Int> offsets =  roomForm.RoomShape?.Offsets ?? new HashSet<Vector2Int>{ Vector2Int.zero };

                foreach (Vector2Int offset in offsets)
                {
                    Vector2Int globalPos = origin + offset;
                    cellMap[globalPos] = room;

                    if (globalPos.x < minX) minX = globalPos.x;
                    if (globalPos.x > maxX) maxX = globalPos.x;
                    if (globalPos.y < minY) minY = globalPos.y;
                    if (globalPos.y > maxY) maxY = globalPos.y;
                }
            }

            int width = maxX - minX + 1;
            int height = maxY - minY + 1;
            var grid = new IGameObjectRoom[width, height];

            foreach (var kvp in cellMap)
            {
                Vector2Int normalizedPos = new Vector2Int(kvp.Key.x - minX, kvp.Key.y - minY);
                grid[normalizedPos.x, normalizedPos.y] = kvp.Value;
            }

            return grid;
        }
    }
}