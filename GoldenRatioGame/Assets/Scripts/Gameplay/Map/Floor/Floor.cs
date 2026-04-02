using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    public class Floor : MonoBehaviour
    {
        public IDataGraph<IRoom> FloorGraph { get; private set; }
        
        public void SetFloorGraph(IDataGraph<IRoom> floorGraph)
        {
            if (FloorGraph != null) throw new InvalidOperationException("Floor Graph can only be set once");
            
            FloorGraph = floorGraph;
        }
        
        public IDataNode<IRoom> GetNode(IRoom room)
        {
            return FloorGraph?.DataNodes.FirstOrDefault(x => x.Value == room);
        }
    }
}