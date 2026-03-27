using System.Collections.Generic;
using System.Linq;
using IM.Factions;
using IM.LifeCycle;
using IM.Modules;
using UnityEngine;

namespace IM
{
    public class ModuleEntityFactory : IFactory<IModuleEntity, ModuleEntityEntry, IGameObjectFactory>
    {
        public IModuleEntity Create(ModuleEntityEntry entry, IGameObjectFactory factory)
        {
            GameObject created = factory.Create(entry.EntityGameObject, false);
            created.transform.position = new Vector3(Random.Range(entry.Bounds.min.x, entry.Bounds.max.x),
                Random.Range(entry.Bounds.min.y, entry.Bounds.max.y));
            IModuleEntity entity = created.GetComponent<IModuleEntity>();
            
            if (entry.Faction&& created.TryGetComponent(out IFactionMember factionMember)) factionMember.Faction = entry.Faction;
            IEnumerable<IExtensibleModule> modules = entry.ModulesGameObjects.Select(x => factory.Create(x, false).GetComponent<IExtensibleModule>());
            
            AddModulesToEntity(entity, modules);

            return entity;
        }
        
        private void AddModulesToEntity(IModuleEntity entity, IEnumerable<IExtensibleModule> modules)
        {
            try
            {
                IGraphOperations graphOperations = 
                    new CommandGraphOperations(entity.ModuleEditingContext.GraphEditor.BeginEdit());
                
                foreach (IExtensibleModule toAdd in modules)
                {
                    entity.ModuleEditingContext.AddToContext(toAdd);
                    graphOperations.TryQuickAddModule(toAdd);
                }
            }
            finally
            {
                if(!entity.ModuleEditingContext.GraphEditor.TryApplyChanges()) entity.ModuleEditingContext.GraphEditor.DiscardChanges();
            }
        }
    }
}