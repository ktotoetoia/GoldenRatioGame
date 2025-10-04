namespace IM.Graphs
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}