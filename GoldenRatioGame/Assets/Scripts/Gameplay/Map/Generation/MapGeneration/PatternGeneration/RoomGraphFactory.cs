using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map.Grid
{
    public class RoomGraphFactory
    {
        private readonly IGameObjectFactory _goFactory;

        public RoomGraphFactory(IGameObjectFactory goFactory) => _goFactory = goFactory;

        public IDataGraph<IGameObjectRoom> Create(MapGenerationContext context)
        {
            var grid = context.Grid;
            var graph = new BiDirectionalDataGraph<IGameObjectRoom>();
            var roomToNode = new Dictionary<int, IDataNode<IGameObjectRoom>>();
            var nodes = new IDataNode<IGameObjectRoom>[grid.Width, grid.Height];

            foreach (Vector2Int pos in grid.OccupiedPositions())
            {
                ICellInfo cell = grid[pos];

                if (roomToNode.TryGetValue(cell.RoomInstanceId, out var existingNode))
                {
                    nodes[pos.x, pos.y] = existingNode;
                    continue;
                }

                var room = cell.Factory.Create(cell.SelectedPattern, _goFactory) as IGameObjectRoom;
                var node = graph.Create(room);

                roomToNode[cell.RoomInstanceId] = node;
                nodes[pos.x, pos.y] = node;
            }

            ConnectNeighbors(graph, nodes, grid);
            return graph;
        }

        private void ConnectNeighbors(
            IDataGraph<IGameObjectRoom> graph,
            IDataNode<IGameObjectRoom>[,] nodes,
            IGrid<ICellInfo> grid)
        {
            Vector2Int[] directions = { Vector2Int.right, Vector2Int.up };

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    if (nodes[x, y] == null) continue;

                    Vector2Int pos = new(x, y);
                    ICellInfo cellA = grid[pos];

                    foreach (Vector2Int dir in directions)
                    {
                        Vector2Int neighborPos = pos + dir;
                        if (!grid.InBounds(neighborPos)) continue;

                        IDataNode<IGameObjectRoom> nodeA = nodes[x, y];
                        IDataNode<IGameObjectRoom> nodeB = nodes[neighborPos.x, neighborPos.y];

                        if (nodeB == null || nodeA == nodeB) continue;

                        ICellInfo cellB = grid[neighborPos];
                        PortSide sideA = PortSideUtility.FromDirection(dir);
                        PortSide sideB = PortSideUtility.Opposite(sideA);

                        TryConnect(graph, nodeA, nodeB, cellA.Offset, cellB.Offset, sideA, sideB);
                    }
                }
            }
        }

        private void TryConnect(
            IDataGraph<IGameObjectRoom> graph,
            IDataNode<IGameObjectRoom> nodeA,
            IDataNode<IGameObjectRoom> nodeB,
            Vector2Int offsetA,
            Vector2Int offsetB,
            PortSide sideA,
            PortSide sideB)
        {
            IRoomPort portA = nodeA.Value.RoomPorts.FirstOrDefault(p => p.PortSide == sideA && p.CellOffset == offsetA);
            IRoomPort portB = nodeB.Value.RoomPorts.FirstOrDefault(p => p.PortSide == sideB && p.CellOffset == offsetB);

            if (portA == null || portB == null) return;

            portA.SetDestination(portB);
            portB.SetDestination(portA);

            graph.Connect(nodeA, nodeB);
        }
    }
}