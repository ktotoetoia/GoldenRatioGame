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
                moduleEditingContext.MutableStorage.ClearAndRemoveCell(storageCell);
                modules.Add(storageCell.Item as IExtensibleItem);
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

        public bool AddToContext(IExtensibleItem module)
        {
            if (module.ItemState == ItemState.Hide || ModuleEditingContextEditor.Snapshot.Storage.ContainsItem(module) || ModuleEditingContextEditor.IsEditing) return false;

            ICellFactoryStorage storage = ModuleEditingContextEditor.BeginEdit().MutableStorage;
            
            storage.SetItem(storage.FirstOrDefault(x => x.Item == null) ?? storage.CreateCell(), module);
            module.ItemState = ItemState.Hide;

            if (ModuleEditingContextEditor.TryApplyChanges()) return true;
            ModuleEditingContextEditor.DiscardChanges();

            return false;
        }

        public bool RemoveFromContext(IExtensibleItem module)
        {
            if ( !ModuleEditingContextEditor.Snapshot.Storage.ContainsItem(module) || ModuleEditingContextEditor.IsEditing) return false;
            
            ICellFactoryStorage storage = ModuleEditingContextEditor.BeginEdit().MutableStorage;
            
            storage.ClearCell(storage.GetCell(module));
            module.ItemState = ItemState.Show;
            
            if (ModuleEditingContextEditor.TryApplyChanges()) return true;
            
            ModuleEditingContextEditor.DiscardChanges();
            
            return false;
        }
    }
}