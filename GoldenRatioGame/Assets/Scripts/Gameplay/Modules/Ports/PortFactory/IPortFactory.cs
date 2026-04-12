using System.Collections.Generic;
using IM.Graphs;
using IM.LifeCycle;

namespace IM.Modules
{
    public interface IPortFactory : IFactory<IEnumerable<IDataPort<IExtensibleItem>>,IDataModule<IExtensibleItem>>
    {
        
    }
}