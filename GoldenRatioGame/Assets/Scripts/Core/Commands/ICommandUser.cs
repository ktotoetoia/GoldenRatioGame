namespace IM.Commands
{
    public interface ICommandUser
    {
        int CommandsToUndoCount { get; }
        int CommandsToRedoCount { get; }

        bool CanUndo(int count);
        bool CanRedo(int count);
        void Undo(int count);
        void Redo(int count);
    }
}