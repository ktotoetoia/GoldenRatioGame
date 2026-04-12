using IM.Commands;

namespace IM.Graphs
{
    public interface IDataConnectCommand<T> : ICommand
    {
        IDataConnection<T> Connection { get; }
    }
}