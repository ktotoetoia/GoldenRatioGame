using System.Collections.Generic;
using IM.Graphs;

namespace IM.Modules
{
    public interface IGameModuleGraph: ICoreModuleGraph
    {
        public IReadOnlyCollection<IModuleObserver> Observers { get; }
        public void AddObserver(IModuleObserver observer);
        public void RemoveObserver(IModuleObserver observer);

        bool AddAndConnect(IModule module, IModulePort modulePort, IModulePort targetPort);
    }
}