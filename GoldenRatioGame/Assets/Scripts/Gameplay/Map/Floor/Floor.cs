using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.LifeCycle;
using IM.Map.Grid;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IM.Map
{
    public class Floor : MonoBehaviour, IFloor,IRequireGameObjectFactory
    {
        private readonly SmartCollection<IRoomWalker> _roomWalkers = new();
        private IMapFactory _mapFactory;
        
        public IDataGraph<IGameObjectRoom> FloorGraph { get; private set; }
        public IEnumerable<IRoomWalker> RoomWalkers => _roomWalkers;
        public int Seed { get; set; }
        public int Depth { get;  set; }
        public int MinRooms { get; set; }
        public int MaxRooms { get;  set; }
        public IGameObjectFactory Factory { get; set; }

        public void SetMapFactory(IMapFactory factory)
        {
            _mapFactory = factory;
        }

        public void Next(IMapInfo mapInfo)
        {
            Clear();
            
            FloorGraph = mapInfo.Graph;
            
            UpdateRoomWalkers();
        }

        public void Next()
        {
            Clear();
            
            FloorGraph = _mapFactory.Create(Factory,Random.Range(MinRooms, MaxRooms), Seed).Graph;

            UpdateRoomWalkers();
        }

        public void AddRoomWalker(IRoomWalker walker)
        { 
            _roomWalkers.Add(walker);

            if (FloorGraph != null)
            {
                walker.GoTo(FloorGraph.DataNodes.FirstOrDefault(x => x.Value is IHaveRoomType { RoomType: RoomType.Start })?.Value ?? FloorGraph.DataNodes.FirstOrDefault()?.Value);
            }
        }

        public void RemoveRoomWalker(IRoomWalker walker)
        {
            _roomWalkers.Remove(walker);
        }
        
        private void Clear()
        {
            foreach (var roomWalker in _roomWalkers)
            {
                roomWalker.GoTo(null);
                if (roomWalker is MonoBehaviour monoBehaviour)
                {
                    monoBehaviour.transform.parent = transform;
                }
            }
            
            if(FloorGraph == null) return;
            
            foreach (MonoBehaviour room in FloorGraph.DataNodes.Select(x => x.Value).OfType<MonoBehaviour>())
            {
                Destroy(room.gameObject);
            }
        }

        private void UpdateRoomWalkers()
        {
            var startingRoom =
                FloorGraph.DataNodes.FirstOrDefault(x => x.Value is IHaveRoomType { RoomType: RoomType.Start })
                    ?.Value ?? FloorGraph.DataNodes.FirstOrDefault()?.Value;
            
            foreach (var roomWalker in _roomWalkers)
            {
                roomWalker.GoTo(startingRoom);
            }
        }
    }
}