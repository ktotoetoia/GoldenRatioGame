using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEditingContextMono : MonoBehaviour, IModuleEditingContext
    {
        [SerializeField] private GameObject _moduleObserversSource;
        [SerializeField] private GameObject _factoriesSource;
        
        private IModuleEditingContext _moduleEditingContext;
        public IModuleGraphEditor<IConditionalCommandModuleGraph> GraphEditor => _moduleEditingContext.GraphEditor;
        public IReadOnlyStorage Storage => _moduleEditingContext.Storage;

        private void Awake()
        {
            _moduleEditingContext = new ModuleEditingContextFactory()
            {
                CreateAddFactory = storage => new AddModuleDirectObserversCommandFactory( _factoriesSource.GetComponents<ICommandObserverAddFactory>().ToList()),
                CreateRemoveFactory = storage =>
                    new RemoveModuleDirectObserversCommandFactory(_factoriesSource.GetComponents<ICommandObserverRemoveFactory>().ToList())
            }.Create(new CellFactoryStorage());
            
            foreach (IEditorObserver<IModuleGraphReadOnly> observer in _moduleObserversSource.GetComponents<IEditorObserver<IModuleGraphReadOnly>>())
            {
                GraphEditor.Observers.Add(observer);
            }
        }
        
        public void AddToContext(IExtensibleModule module) => _moduleEditingContext.AddToContext(module);
        public void RemoveFromContext(IExtensibleModule module) => _moduleEditingContext.RemoveFromContext(module);
    }
}