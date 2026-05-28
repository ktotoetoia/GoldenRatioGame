namespace IM.Map
{
    public interface IRoomForm
    {
        IRoomShape RoomShape { get; }
        void Apply(IRoomShape shape);
    }
}