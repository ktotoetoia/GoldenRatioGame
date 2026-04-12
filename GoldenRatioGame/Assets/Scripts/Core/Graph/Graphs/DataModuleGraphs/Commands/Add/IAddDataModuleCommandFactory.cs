using System.Collections.Generic;
using IM.Commands;
using IM.LifeCycle;

namespace IM.Graphs
{
    public interface IAddDataModuleCommandFactory<T> : IFactory<ICommand, IDataModule<T>, ICollection<IDataModule<T>>>
    {

    }
}