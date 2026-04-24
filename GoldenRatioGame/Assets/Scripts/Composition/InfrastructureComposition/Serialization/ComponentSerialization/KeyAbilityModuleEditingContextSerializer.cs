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

        public override void RestoreState(
            ModuleEditingContextEditorMono component,
            object state,
            Func<string, GameObject> resolveDependency)
        {
            if (state is not ModuleContextData savedState)
                return;

            var context = component.BeginEdit();
            context.SetUnsafe(true);

            try
            {
                var graphIds = savedState.GraphData.ModuleInfos
                    .Select(m => m.ItemId)
                    .Where(id => !string.IsNullOrEmpty(id))
                    .ToHashSet();

                foreach (var id in savedState.StorageModuleIds)
                {
                    if (string.IsNullOrEmpty(id) || graphIds.Contains(id))
                        continue;

                    var prefab = resolveDependency(id);
                    if (prefab != null && prefab.TryGetComponent(out IItem item))
                    {
                        context.AddToContext(item);
                    }
                }

                _graphSerializer.Deserialize(savedState.GraphData, context.ModuleGraph, context, resolveDependency);
            }
            finally
            {
                context.SetUnsafe(false);

                if (!component.TryApplyChanges())
                    component.DiscardChanges();
            }
        }
    }
}