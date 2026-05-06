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

        public IDataGraph<IGameObjectRoom> Create(RoomGrid grid)
        {
            var graph = new BiDirectionalDataGraph<IGameObjectRoom>();
            var nodes = new IDataNode<IGameObjectRoom>[grid.Width, grid.Height];

            foreach (Vector2Int pos in grid.OccupiedPositions())
            {
                CellInfo cell = grid[pos];
                var room = cell.Factory.Create(cell.SelectedPattern, _goFactory) as IGameObjectRoom;
                nodes[pos.x, pos.y] = graph.Create(room);
            }

            ConnectNeighbors(graph, nodes, grid);
            return graph;
        }

        private void ConnectNeighbors(
            BiDirectionalDataGraph<IGameObjectRoom> graph,
            IDataNode<IGameObjectRoom>[,] nodes,
            RoomGrid grid)
        {
            Vector2Int[] directions = { Vector2Int.right, Vector2Int.up };

            foreach (Vector2Int pos in grid.OccupiedPositions())
            {
                foreach (Vector2Int dir in directions)
                {
                    Vector2Int neighborPos = pos + dir;
                    if (!grid.InBounds(neighborPos)) continue;
                    if (nodes[neighborPos.x, neighborPos.y] == null) continue;

                    TryConnect(
                        graph,
                        nodes[pos.x, pos.y],
                        nodes[neighborPos.x, neighborPos.y],
                        PortSideUtility.FromDirection(dir),
                        PortSideUtility.FromDirection(-dir));
                }
            }
        }

        private void TryConnect(
            BiDirectionalDataGraph<IGameObjectRoom> graph,
            IDataNode<IGameObjectRoom> nodeA,
            IDataNode<IGameObjectRoom> nodeB,
            PortSide sideA, PortSide sideB)
        {
            IRoomPort portA = nodeA.Value.RoomPorts.FirstOrDefault(p => p.PortSide == sideA);
            IRoomPort portB = nodeB.Value.RoomPorts.FirstOrDefault(p => p.PortSide == sideB);

            if (portA == null || portB == null) return;

            portA.SetDestination(portB);
            portB.SetDestination(portA);
            graph.Connect(nodeA, nodeB);
        }
    }
}