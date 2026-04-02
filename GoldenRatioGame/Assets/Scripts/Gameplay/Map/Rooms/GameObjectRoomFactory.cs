namespace IM.Map
{
    public class GameObjectRoomFactory : IRoomFactory
    {
        public GameObjectRoomFactory()
        {
            
        }
        
        public IRoom Create()
        {
            return new GameObjectRoom();
        }
    }
}