using System.Collections.Generic;

namespace IM.Graphs
{
    public class GridGraph<T> : IGraphReadOnly<T>
    {
        private readonly Node<T>[,] _nodeMatrix;
        private readonly List<Node<T>> _nodes = new();
        private readonly List<Edge<T>> _edges = new();

        public IReadOnlyList<INode<T>> Nodes => _nodes;
        public IReadOnlyList<IEdge<T>> Edges => _edges;
        public INode<T>[,] NodeMatrix => _nodeMatrix;

        public int Width { get; }
        public int Height { get; }

        public GridGraph(int width, int height)
        {
            Width = width;
            Height = height;

            _nodeMatrix = new Node<T>[Width, Height];
            CreateNodes();
            ConnectNodes();
        }

        private void CreateNodes()
        {
            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                Node<T> node = new Node<T>();
                _nodeMatrix[x, y] = node;
                _nodes.Add(node);
            }
        }

        private void ConnectNodes()
        {
            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                Node<T> current = _nodeMatrix[x, y];

                if (x + 1 < Width)
                    AddEdge(current, _nodeMatrix[x + 1, y]);

                if (y + 1 < Height)
                    AddEdge(current, _nodeMatrix[x, y + 1]);
            }
        }

        private void AddEdge(Node<T> a, Node<T> b)
        {
            Edge<T> edge = new Edge<T>(a, b);
            _edges.Add(edge);
            a.Add(edge);
            b.Add(edge);
        }
    }
}