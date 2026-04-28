using System.Collections.Generic;
using System.Linq;
using IM.Factions;
using IM.LifeCycle;
using IM.Modules;
using UnityEngine;
using Random = UnityEngine.Random;

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
            IEnumerable<IExtensibleItem> modules = entry.ModulesGameObjects.Select(x => factory.Create(x, false).GetComponent<IExtensibleItem>()).ToList();
            
            AddModulesToEntity(entity, modules);

            return entity;
        }
        
        private void AddModulesToEntity(IModuleEntity entity, IEnumerable<IExtensibleItem> items)
        {
            try
            {
                List<IExtensibleItem> itemsList = items.ToList();
                
                foreach (IExtensibleItem toAdd in itemsList)
                {
                    entity.AddToContext(toAdd);
                }
                
                IModuleEditingContext moduleEditingContext = entity.ModuleEditingContextEditor.BeginEdit();

                foreach (IExtensibleItem toAdd in itemsList)
                {
                    moduleEditingContext.GraphEditing.TryQuickAddModule(
                        moduleEditingContext.GraphEditing.CreateModule(toAdd));
                }
            }
            finally
            {
                if(!entity.ModuleEditingContextEditor.TryApplyChanges()) entity.ModuleEditingContextEditor.DiscardChanges();
            }
        }
    }
}