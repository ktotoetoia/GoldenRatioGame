using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IObservableModuleGraph: IModuleGraph
    {
        public IReadOnlyCollection<IModuleObserver> Observers { get; }
        public void AddObserver(IModuleObserver observer);
        public void RemoveObserver(IModuleObserver observer);
    }
    
    public interface IGameModuleGraph : IObservableModuleGraph
    {
        bool AddAndConnect(IModule module, IModulePort modulePort, IModulePort targetPort);
    }
}