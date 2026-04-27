using System;
using System.Collections.Generic;
using System.Linq;
using IM.Items;
using IM.LifeCycle;
using IM.SaveSystem;
using UnityEngine;

namespace IM.Modules
{
    public sealed class ModuleEditingContextEditorMonoSerializer : ComponentSerializer<ModuleEditingContextEditorMono>
    {
        [Serializable]
        public class ModuleContextData
        {
            public List<string> StorageModuleIds = new();
            public GraphSerializer.GraphInfo GraphData = new();
        }

        private readonly GraphSerializer _graphSerializer = new();

        public override object CaptureState(ModuleEditingContextEditorMono component)
        {
            var snapshot = component.Snapshot;

            var state = new ModuleContextData
            {
                GraphData = _graphSerializer.Serialize(snapshot.Graph)
            };

            state.StorageModuleIds.AddRange(
                snapshot.Storage
                    .Select(cell => (cell.Item as IEntity).GetModuleId())
                    .Where(id => !string.IsNullOrEmpty(id)));

            return state;
        }

        public override void RestoreState(ModuleEditingContextEditorMono component, object state, Func<string, GameObject> resolveDependency)
        {
            if (state is not ModuleContextData savedState) return;

            var context = component.BeginEdit();
            
            try
            {
                var graphIds = savedState.GraphData.ModuleInfos
                    .Select(m => m.ItemId)
                    .Where(id => !string.IsNullOrEmpty(id))
                    .ToHashSet();

                foreach (GameObject prefab in savedState.StorageModuleIds.Where(id => !string.IsNullOrEmpty(id) && !graphIds.Contains(id)).Select(resolveDependency))
                {
                    if (!prefab || !prefab.TryGetComponent(out IItem item)) continue;
                    
                    context.AddToContext(item);
                }

                _graphSerializer.Deserialize(savedState.GraphData, context.Services.Get<UnsafeGraphEditingService<IExtensibleItem>>(), context, resolveDependency);
            }
            finally
            {
                if (!component.TryApplyChanges()) component.DiscardChanges();
            }
        }
    }
}