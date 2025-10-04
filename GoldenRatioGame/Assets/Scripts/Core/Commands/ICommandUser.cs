namespace IM.Commands
{
    public interface ICommandUser
    {
        bool CanUndo { get; }
        bool CanRedo { get; }
        
        void Undo(int count);
        void Redo(int count);
    }
}