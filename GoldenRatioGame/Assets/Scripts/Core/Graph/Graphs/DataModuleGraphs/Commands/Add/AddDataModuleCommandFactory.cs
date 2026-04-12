using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class AddDataModuleCommandFactory<T> : IAddDataModuleCommandFactory<T>
    {
        public ICommand Create(IDataModule<T> param1, ICollection<IDataModule<T>> param2)
        {
            return new AddDataModuleCommand<T>(param1,param2);
        }
    }
}