using System.Collections.Generic;

namespace IM.Graphs
{
    public class GridGraph : IGraphReadOnly
    {
        private readonly Node[,] _nodeMatrix;
        private readonly List<Node> _nodes = new();
        private readonly List<Edge> _edges = new();

        public IReadOnlyList<INode> Nodes => _nodes;
        public IReadOnlyList<IEdge> Edges => _edges;
        public INode[,] NodeMatrix => _nodeMatrix;

        public int Width { get; }
        public int Height { get; }

        public GridGraph(int width, int height)
        {
            Width = width;
            Height = height;

            _nodeMatrix = new Node[Width, Height];
            CreateNodes();
            ConnectNodes();
        }

        private void CreateNodes()
        {
            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                Node node = new Node();
                _nodeMatrix[x, y] = node;
                _nodes.Add(node);
            }
        }

        private void ConnectNodes()
        {
            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                Node current = _nodeMatrix[x, y];

                if (x + 1 < Width)
                    AddEdge(current, _nodeMatrix[x + 1, y]);

                if (y + 1 < Height)
                    AddEdge(current, _nodeMatrix[x, y + 1]);
            }
        }

        private void AddEdge(Node a, Node b)
        {
            Edge edge = new Edge(a, b);
            _edges.Add(edge);
            a.Add(edge);
            b.Add(edge);
        }
    }
}