using System;
using System.Collections.Generic;
using System.Linq;
using IM.Entities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class EntityInjector : IModuleGraphObserver
    {
        private readonly HashSet<IRequireEntity> _injected = new();
        private readonly IEntity _entity;

        public EntityInjector(IEntity entity)
        {
            _entity = entity ?? throw new ArgumentNullException(nameof(entity));
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));

            HashSet<IRequireEntity> current = new();

            foreach (var module in graph.Modules)
            {
                if (module is IRequireEntity requireEntity)
                {
                    current.Add(requireEntity);
                }
            }

            foreach (var requireEntity in current)
            {
                if (_injected.Add(requireEntity))
                {
                    if (requireEntity.Entity != null)
                    {
                        Debug.LogWarning($"Entity already set on {requireEntity}. Overwriting with new entity.");
                    }

                    requireEntity.Entity = _entity;
                }
            }

            foreach (var requireEntity in _injected.Except(current).ToList())
            {
                requireEntity.Entity = null;
                _injected.Remove(requireEntity);
            }
        }
    }
}