using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Items;
using IM.LifeCycle;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    [DisallowMultipleComponent]
    public class ModuleEntity : DefaultEntity, IModuleEntity
    {
        public IModuleEditingContextEditor ModuleEditingContextEditor { get; private set; }
        public event Action<IEnumerable<IExtensibleItem>> Disassembled;

        private void Awake()
        {
            ModuleEditingContextEditor = GetComponent<IModuleEditingContextEditor>();
        }

        public override void Destroy()
        {
            List<IExtensibleItem> modules = ExtractExtensibleItems();
            
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

        private List<IExtensibleItem> ExtractExtensibleItems()
        {
            IModuleEditingContext moduleEditingContext = ModuleEditingContextEditor.BeginEdit();
            
            SafeClearGraph(moduleEditingContext.ModuleGraph);
            moduleEditingContext.SetUnsafe(true);
            UnsafeClearGraph(moduleEditingContext.ModuleGraph);
            moduleEditingContext.SetUnsafe(false);
            
            List<IExtensibleItem> modules = new List<IExtensibleItem>();
            
            foreach (IStorageCellReadonly storageCell in moduleEditingContext.MutableStorage.Where(x => x.Item is IExtensibleItem).ToList())
            {
                if (storageCell.Item is IExtensibleItem item)
                {
                    moduleEditingContext.RemoveFromContext(item);
                    modules.Add(item);
                }
            }
            
            if(!ModuleEditingContextEditor.TryApplyChanges()) ModuleEditingContextEditor.DiscardChanges();
            
            return modules;
        }

        private void SafeClearGraph(IConditionalCommandDataModuleGraph<IExtensibleItem> graph)
        {
            int removedCount = 0;

            do
            {
                removedCount = 0;

                foreach (IDataModule<IExtensibleItem> module in graph.DataModules.ToList())
                {
                    if (graph.CanRemove(module))
                    {
                        graph.Remove(module);
                        removedCount++;
                    }
                }
            }
            while(removedCount > 0);
        }

        private void UnsafeClearGraph(IConditionalCommandDataModuleGraph<IExtensibleItem> graph)
        {
            if (graph.Modules.Count <= 0) return;

            foreach (IDataModule<IExtensibleItem> module in graph.DataModules.ToList()) graph.Remove(module);
        }

        public bool AddToContext(IItem module)
        {
            if (module.ItemState == ItemState.Hide || ModuleEditingContextEditor.IsEditing) return false;

            if (ModuleEditingContextEditor.BeginEdit().AddToContext(module) &&
                ModuleEditingContextEditor.TryApplyChanges())
            {
                return true;
            }
            
            ModuleEditingContextEditor.DiscardChanges();

            return false;
        }

        public bool RemoveFromContext(IItem module)
        {
            if (ModuleEditingContextEditor.IsEditing) return false;
            
            if (ModuleEditingContextEditor.BeginEdit().RemoveFromContext(module) &&
                ModuleEditingContextEditor.TryApplyChanges())
            {
                return true;
            }
            
            ModuleEditingContextEditor.DiscardChanges();

            return false;
        }
    }
}