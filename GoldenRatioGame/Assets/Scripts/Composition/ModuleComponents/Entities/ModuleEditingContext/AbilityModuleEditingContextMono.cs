using System.Linq;
using IM.Abilities;
using IM.Graphs;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    public class AbilityModuleEditingContextMono : MonoBehaviour, IAbilityPoolModuleEditingContext
    {
        [SerializeField] private GameObject _moduleObserversSource;
        [SerializeField] private GameObject _factoriesSource;
        private AbilityPool _abilityPool;
        private ModuleEditingContext _moduleEditingContext;
        
        public bool IsUnsafe => _moduleEditingContext.IsUnsafe;
        public void SetUnsafe(bool value) => _moduleEditingContext.SetUnsafe(value);

        public IModuleGraphEditor<IConditionalCommandModuleGraph> GraphEditor => _moduleEditingContext.GraphEditor;
        public IReadOnlyStorage Storage => _moduleEditingContext.Storage;
        public IAbilityPool KeyAbilityPool => _abilityPool??= new AbilityPool();
        
        private void Awake()
        {
            _moduleEditingContext = new ModuleEditingContextFactory
            {
                CreateAddFactory = storage => 
                    new AddModuleDirectObserversCommandFactory( _factoriesSource.GetComponents<ICommandObserverAddFactory>().ToList()),
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