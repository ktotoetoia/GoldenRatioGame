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
            
            SafeClearGraph(moduleEditingContext.GraphEditing);
            UnsafeClearGraph(moduleEditingContext.UnsafeGraphEditing);
            
            List<IExtensibleItem> modules = new List<IExtensibleItem>();
            
            foreach (IStorageCellReadonly storageCell in moduleEditingContext.Storage.Where(x => x.Item is IExtensibleItem).ToList())
            {
                if (storageCell.Item is not IExtensibleItem item) continue;
                
                moduleEditingContext.StorageEditing.AddToStorage(item);
                modules.Add(item);
            }
            
            List<IExtensibleItem> finalModules = new List<IExtensibleItem>();
            
            foreach (IStorageCellReadonly storageCellReadonly in moduleEditingContext.Storage)
            {
                if (storageCellReadonly.Item is IExtensibleItem item && moduleEditingContext.StorageEditing.RemoveFromStorage(item))
                {
                    finalModules.Add(item);
                }
            }
            
            if(!ModuleEditingContextEditor.TryApplyChanges()) ModuleEditingContextEditor.DiscardChanges();
            
            return finalModules;
        }

        private void SafeClearGraph(IGraphEditingService<IExtensibleItem> graphEditingService)
        {
            int removedCount = 0;

            do
            {
                removedCount = 0;

                foreach (var module in graphEditingService.GraphReadOnly.DataModules.ToList().Where(graphEditingService.CanRemove))
                {
                    graphEditingService.Remove(module);
                    removedCount++;
                }
            }
            while(removedCount > 0);
        }

        private void UnsafeClearGraph(IGraphEditingService<IExtensibleItem> graphEditingService)
        {
            if (graphEditingService.GraphReadOnly.Modules.Count <= 0) return;

            foreach (IDataModule<IExtensibleItem> module in graphEditingService.GraphReadOnly.DataModules.ToList()) graphEditingService.Remove(module);
        }

        public bool AddToContext(IItem module)
        {
            if (module.Owner != null || ModuleEditingContextEditor.IsEditing) return false;

            if (ModuleEditingContextEditor.BeginEdit().StorageEditing.AddToStorage(module) &&
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
            
            if (ModuleEditingContextEditor.BeginEdit().StorageEditing.RemoveFromStorage(module) &&
                ModuleEditingContextEditor.TryApplyChanges())
            {
                return true;
            }
            
            ModuleEditingContextEditor.DiscardChanges();

            return false;
        }
    }
}