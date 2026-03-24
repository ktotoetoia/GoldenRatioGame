using System.Collections.Generic;
using System.Linq;
using IM.Entities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    [DisallowMultipleComponent]
    public class ModuleEntity : DefaultEntity, IModuleEntity
    {
        [SerializeField] private int _dropCount = 2;
        
        public IModuleEditingContext ModuleEditingContext { get; private set; }

        private void Awake()
        {
            ModuleEditingContext = GetComponent<IModuleEditingContext>();
        }
        
        [ContextMenu("Destroy")]
        public override void Destroy()
        {
            List<IExtensibleModule> modules = Disassemble();
            
            foreach (IExtensibleModule module in ModuleEditingContext.Storage.Select(x=> x.Item).OfType<IExtensibleModule>())
            {
                ModuleEditingContext.RemoveFromContext(module);
            }

            for (int i = 0; i < _dropCount && modules.Count > 0; i++)
            {
                IExtensibleModule module = modules[Random.Range(0, modules.Count)];
                modules.Remove(module);
            }
            
            foreach (IExtensibleModule module in modules)
            {
                module.Destroy();
            }
            
            base.Destroy();
        }

        private List<IExtensibleModule> Disassemble()
        {
            ModuleEditingContext.SetUnsafe(true);
            List<IExtensibleModule> result = new();
            
            IConditionalCommandModuleGraph graph = ModuleEditingContext.GraphEditor.BeginEdit();

            foreach (IModule module in graph.Modules.ToList())
            {
                graph.RemoveModule(module);
                result.Add(module as IExtensibleModule);
            }

            ModuleEditingContext.GraphEditor.TryApplyChanges();

            return result;
        }
    }
}