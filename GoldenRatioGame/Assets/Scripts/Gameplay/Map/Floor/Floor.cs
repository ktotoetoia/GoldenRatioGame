using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.LifeCycle;
using IM.Map.Grid;
using UnityEngine;

namespace IM.Map
{
    public class Floor : MonoBehaviour, IFloor,IRequireGameObjectFactory
    {
        private readonly SmartCollection<IRoomWalker> _roomWalkers = new();
        private IMapInfo _mapInfo;

        public IDataGraph<IGameObjectRoom> FloorGraph => _mapInfo?.Graph;
        public IEnumerable<IRoomWalker> RoomWalkers => _roomWalkers;
        public int Seed { get; set; }
        public int Depth { get;  set; }
        public IGameObjectFactory Factory { get; set; }
        public IMapInfoFactory MapInfoFactory {get; private set; }

        public void SetMapFactory(IMapInfoFactory infoFactory)
        {
            MapInfoFactory = infoFactory;
        }

        public void Next(IMapInfo mapInfo)
        {
            Clear();
            
            _mapInfo = mapInfo;
            if (_mapInfo.FinalRoom is IHaveTransitionPoint transitionPoint)
            {
                transitionPoint.TransitionPoint.Interacted += Next;
            }
            
            UpdateRoomWalkers();
        }

        public void Next()
        {
            Clear();
            
            Depth++;
            _mapInfo = MapInfoFactory.Create(Factory,Seed,Depth); 
            
            if (_mapInfo.FinalRoom is IHaveTransitionPoint transitionPoint)
            {
                transitionPoint.TransitionPoint.Interacted += Next;
            }

            UpdateRoomWalkers();
        }

        public void AddRoomWalker(IRoomWalker walker)
        { 
            AddRoomWalker(walker,_mapInfo?.StartRoom);
        }

        public void AddRoomWalker(IRoomWalker walker, IGameObjectRoom room)
        {
            _roomWalkers.Add(walker);

            if (room != null)
            {
                walker.GoTo(room);
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
            
            if(_mapInfo == null) return;
            
            foreach (MonoBehaviour room in FloorGraph.DataNodes.Select(x => x.Value).OfType<MonoBehaviour>())
            {
                Destroy(room.gameObject);
            }
        }

        private void UpdateRoomWalkers()
        {
            foreach (var roomWalker in _roomWalkers)
            {
                roomWalker.GoTo(_mapInfo.StartRoom);
            }
        }
    }
}