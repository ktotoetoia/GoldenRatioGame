using System;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Map
{
    public class Floor : MonoBehaviour, IFloor
    {
        public IDataGraph<IGameObjectRoom> FloorGraph { get; private set; }
        
        public void SetFloorGraph(IDataGraph<IGameObjectRoom> floorGraph)
        {
            if (FloorGraph != null) throw new InvalidOperationException("Floor Graph can only be set once");
            
            FloorGraph = floorGraph;
        }
        
        public IDataNode<IGameObjectRoom> GetNode(IGameObjectRoom room)
        {
            return FloorGraph?.DataNodes.FirstOrDefault(x => x.Value == room);
        }
    }
}