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

            var portLookup = BuildPortLookup(grid, roomToNode);

            ConnectNeighbors(graph, nodes, grid, portLookup);
            return graph;
        }

        private Dictionary<(int RoomId, Vector2Int Offset, PortSide Side), IRoomPort> BuildPortLookup(
            IGrid<ICellInfo> grid, 
            Dictionary<int, IDataNode<IGameObjectRoom>> roomToNode)
        {
            var portLookup = new Dictionary<(int, Vector2Int, PortSide), IRoomPort>();

            var roomGroups = grid.OccupiedPositions()
                .Select(pos => (pos, cell: grid[pos]))
                .GroupBy(x => x.cell.RoomInstanceId);

            foreach (var group in roomGroups)
            {
                int roomId = group.Key;
                var room = roomToNode[roomId].Value;

                var portsByIndex = room.RoomPorts.ToDictionary(p => p.PortIdentity.Index);

                foreach (var item in group)
                {
                    Vector2Int cellOffset = item.cell.Offset;
                    if (!item.cell.SelectedPattern.PortDefinitions.TryGetValue(cellOffset, out var portDefs)) continue;

                    foreach (IPortDefinition portDef in portDefs)
                    {
                        if (portsByIndex.TryGetValue(portDef.Index, out var runtimePort))
                        {
                            portLookup[(roomId, cellOffset, portDef.Side)] = runtimePort;
                        }
                    }
                }
            }

            return portLookup;
        }

        private void ConnectNeighbors(
            IDataGraph<IGameObjectRoom> graph,
            IDataNode<IGameObjectRoom>[,] nodes,
            IGrid<ICellInfo> grid,
            Dictionary<(int RoomId, Vector2Int Offset, PortSide Side), IRoomPort> portLookup)
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

                        TryConnect(graph, nodeA, nodeB, cellA, cellB, sideA, sideB, portLookup);
                    }
                }
            }
        }

        private void TryConnect(
            IDataGraph<IGameObjectRoom> graph,
            IDataNode<IGameObjectRoom> nodeA,
            IDataNode<IGameObjectRoom> nodeB,
            ICellInfo cellA,
            ICellInfo cellB,
            PortSide sideA,
            PortSide sideB,
            Dictionary<(int RoomId, Vector2Int Offset, PortSide Side), IRoomPort> portLookup)
        {
            portLookup.TryGetValue((cellA.RoomInstanceId, cellA.Offset, sideA), out var portA);
            portLookup.TryGetValue((cellB.RoomInstanceId, cellB.Offset, sideB), out var portB);

            if (portA == null || portB == null) return;

            portA.SetDestination(portB);
            portB.SetDestination(portA);

            graph.Connect(nodeA, nodeB);
        }
    }
}