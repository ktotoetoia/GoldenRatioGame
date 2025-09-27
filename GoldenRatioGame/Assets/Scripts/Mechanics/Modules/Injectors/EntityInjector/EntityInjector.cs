using System.Collections.Generic;
using IM.Entities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class EntityInjector : IModuleObserver
    {
        private readonly List<IRequireEntity> _modules = new();
        private readonly IEntity _entity;

        public EntityInjector(IEntity entity)
        {
            _entity = entity;
        }
        
        public void Add(IModule module)
        {
            if (module is not IRequireEntity requireEntity || _modules.Contains(requireEntity))
            {
                return;
            }

            if (requireEntity.Entity != null)
            {
                Debug.LogWarning("entity of a module should be removed before adding module to another entity");
            }
            
            requireEntity.Entity = _entity;
            _modules.Add(requireEntity);
        }

        public void Remove(IModule module)
        {
            if (module is not IRequireEntity requireEntity || !_modules.Contains(requireEntity))
            {
                return;
            }
            
            requireEntity.Entity = null;
            _modules.Remove(requireEntity);
        }
    }
}