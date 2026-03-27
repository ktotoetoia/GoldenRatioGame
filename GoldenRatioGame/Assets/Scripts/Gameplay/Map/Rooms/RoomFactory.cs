using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    public class RoomFactory : IRoomFactory
    {
        private readonly IRoomInitializer _roomInitializer;

        public RoomFactory(IRoomInitializer roomInitializer)
        {
            _roomInitializer = roomInitializer;
        }
        
        public IRoom Create()
        {
            return new GameObjectRoom(_roomInitializer);
        }
    }
}