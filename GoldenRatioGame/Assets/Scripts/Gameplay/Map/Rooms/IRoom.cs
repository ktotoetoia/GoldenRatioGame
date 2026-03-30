namespace IM.Map
{
    public interface IRoom
    {
        void Add(IRoomVisitor roomVisitor);
        void Remove(IRoomVisitor roomVisitor);
        void Enter(IRoomWalker roomWalker);
        void Exit(IRoomWalker roomWalker);
    }
}