using System.Linq;
using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public class ModuleStorageController : IEditorObserver<IModuleEditingContext>
    {
        private readonly ModuleGraphSnapshotValueDiffer<IExtensibleItem> _differ = new();
        private IModuleEditingContext _context;

        public ModuleStorageController()
        {
            _differ.ValueAdded += added =>
            {
                if(_context == null) return;
                
                _context.MutableStorage.ClearCell(_context.MutableStorage.FirstOrDefault(y => y.Item == added));
            };
            
            _differ.ValueRemoved += removed =>
            {
                if(_context == null) return;

                IStorageCellReadonly cell = _context.MutableStorage.FirstOrDefault(x => x.Item == null) ??
                                            _context.MutableStorage.CreateCell();
                
                _context.MutableStorage.SetItem(cell,removed);
            };
        }
        
        public void OnSnapshotChanged(IModuleEditingContext snapshot)
        {
            _context = snapshot;
            _differ.OnSnapshotChanged(snapshot.Graph);
        }
    }
}