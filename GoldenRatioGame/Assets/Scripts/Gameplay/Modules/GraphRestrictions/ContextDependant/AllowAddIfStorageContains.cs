using System.Linq;
using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public class AllowAddIfStorageContains<T> : IDataModuleGraphConditions<T>
    {
        private readonly IReadOnlyStorage _storage;

        public AllowAddIfStorageContains(IReadOnlyStorage storage)
        {
            _storage = storage;
        }

        public bool CanAdd(IDataModule<T> module) => _storage.Any(x => x.Item?.Equals(module.Value) ?? false);
        public bool CanAddAndConnect(IDataModule<T> module, IDataPort<T> ownerPort, IDataPort<T> targetPort) => CanAdd(module);
    }
}