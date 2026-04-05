namespace IM.Map
{
    public interface IRoomVisitor
    {
        
        IRoom CurrentRoom { get; set; }
        bool ActiveInRoom { get; set; }
    }
}