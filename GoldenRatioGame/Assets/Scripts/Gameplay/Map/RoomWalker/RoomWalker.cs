using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Map
{
    public class RoomWalker : IRoomWalker
    {
        private readonly IDataGraphReadOnly<IRoom> _roomGraph;
        private IDataNode<IRoom> _currentNode;
        
        public IEnumerable<IRoom> Available => _currentNode.DataEdges.Select(x => (x.GetOther( _currentNode) as IDataNode<IRoom>).Value);
        public IRoom Current => _currentNode.Value;
        
        public RoomWalker(IDataNode<IRoom> startingNode)
        {
            _currentNode = startingNode;
            _currentNode.Value.Enter();
        }

        public void GoTo(IRoom room)
        {
            if (!TryGetNode(out IDataNode<IRoom> node, room))
            {
                Debug.LogWarning("cannot go to requested room");
                return;
            }
            
            _currentNode.Value.Exit();
            _currentNode = node;
            _currentNode.Value.Enter();
        }

        private bool TryGetNode(out IDataNode<IRoom> node, IRoom room)
        {
            node = _currentNode.DataEdges.Select(x => x.GetOther(_currentNode) as IDataNode<IRoom>).FirstOrDefault(x => x.Value == room);

            return node != null;
        }
    }
}