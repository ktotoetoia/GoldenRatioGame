using System;
using System.Linq;
using IM.Entities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class EntityInjector : IModuleGraphObserver
    {
        private readonly IEntity _entity;
        private readonly EnumerableDiffTracker<IRequireEntity> _diffTracker = new (); 

        public EntityInjector(IEntity entity)
        {
            _entity = entity ?? throw new ArgumentNullException(nameof(entity));
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            
            DiffResult<IRequireEntity> diffResult = _diffTracker.Update(graph.Modules.Where(x => x is IGameModule module && module.Extensions.HasExtensionOfType<IRequireEntity>())
                .SelectMany(x => (x as IGameModule).Extensions.GetExtensions<IRequireEntity>()));
            
            foreach (IRequireEntity requireEntity in diffResult.Added)
            {
                if (requireEntity.Entity != null)
                {
                    Debug.LogWarning($"Entity already set on {requireEntity}. Overwriting with new entity.");
                }

                requireEntity.Entity = _entity;
            }
            
            foreach (IRequireEntity requireEntity in diffResult.Removed)
            {
                requireEntity.Entity = null;
            }
        }
    }
}