using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Modules
{
    [DisallowMultipleComponent]
    public class ModuleEntity : DefaultEntity, IModuleEntity
    {
        public IModuleEditingContext ModuleEditingContext { get; private set; }
        public event Action<IEnumerable<IExtensibleModule>> Disassembled;

        private void Awake()
        {
            ModuleEditingContext = GetComponent<IModuleEditingContext>();
        }

        public override void Destroy()
        {
            List<IExtensibleModule> modules = ExtractExtensibleModules();
            
            try
            {
                Disassembled?.Invoke(modules);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            base.Destroy();
        }

        private List<IExtensibleModule> ExtractExtensibleModules()
        {
            IModuleGraphEditor<IConditionalCommandModuleGraph> editor = ModuleEditingContext.GraphEditor;
            
            IConditionalCommandModuleGraph graph = editor.BeginEdit();
            int removedCount = 0;
            
            do
            {
                removedCount = 0;

                foreach (IModule module in graph.Modules.ToList())
                {
                    if (graph.CanRemoveModule(module))
                    {
                        graph.RemoveModule(module);
                        removedCount++;
                    }
                }
            }
            while(removedCount > 0);

            if(!editor.TryApplyChanges()) editor.DiscardChanges();

            if (graph.Modules.Count > 0)
            {
                ModuleEditingContext.SetUnsafe(true);
            
                graph = editor.BeginEdit();

                List<IModule> allModules = graph.Modules.ToList();
            
                foreach (IModule m in allModules) graph.RemoveModule(m);

                editor.TryApplyChanges();
                ModuleEditingContext.SetUnsafe(false);
            }
            
            List<IExtensibleModule> modules = new List<IExtensibleModule>();
            
            foreach (IExtensibleModule module in ModuleEditingContext.Storage.Select(x => x.Item as IExtensibleModule))
            {
                ModuleEditingContext.RemoveFromContext(module);
                modules.Add(module);
            }
            
            return modules;
        }
    }
}