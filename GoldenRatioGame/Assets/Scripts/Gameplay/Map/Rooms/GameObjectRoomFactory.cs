namespace IM.Map
{
    public class GameObjectRoomFactory : IRoomFactory
    {
        private readonly IRoomInitializer _roomInitializer;

        public GameObjectRoomFactory(IRoomInitializer roomInitializer)
        {
            _roomInitializer = roomInitializer;
        }
        
        public IRoom Create()
        {
            return new GameObjectRoom(_roomInitializer);
        }
    }
}