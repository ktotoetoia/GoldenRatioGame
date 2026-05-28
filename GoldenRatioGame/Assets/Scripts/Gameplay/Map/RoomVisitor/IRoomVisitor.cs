namespace IM.Map
{
    public interface IRoomVisitor
    {
        IGameObjectRoom CurrentRoom { get; set; }
    }
}