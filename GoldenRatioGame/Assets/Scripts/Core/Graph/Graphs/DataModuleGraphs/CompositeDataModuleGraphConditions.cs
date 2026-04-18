using System.Collections.Generic;
using System.Linq;

namespace IM.Graphs
{
    public class CompositeDataModuleGraphConditions<T> : IDataModuleGraphConditions<T>
    {
        private readonly IEnumerable<IDataModuleGraphConditions<T>> _conditions;
        public bool Disable { get; set; }

        public CompositeDataModuleGraphConditions(IEnumerable<IDataModuleGraphConditions<T>> conditions)
        {
            _conditions = conditions;
        }

        public bool CanAdd(IDataModule<T> module)
        {
            if (Disable) return true;
            
            return _conditions.All(x => x.CanAdd(module));
        }

        public bool CanRemove(IDataModule<T> module)
        {
            if (Disable) return true;

            return _conditions.All(x => x.CanRemove(module));
        }

        public bool CanConnect(IDataPort<T> output, IDataPort<T> input)
        {
            if (Disable) return true;
            
            return _conditions.All(x => x.CanConnect(output, input));
        }

        public bool CanDisconnect(IDataConnection<T> connection)
        {
            if (Disable) return true;
            
            return _conditions.All(x => x.CanDisconnect(connection));
        }

        public bool CanAddAndConnect(IDataModule<T> module, IDataPort<T> ownerPort, IDataPort<T> targetPort)
        {
            if (Disable) return true;
            
            return _conditions.All(x => x.CanAddAndConnect(module, ownerPort, targetPort));
        }
    }
}