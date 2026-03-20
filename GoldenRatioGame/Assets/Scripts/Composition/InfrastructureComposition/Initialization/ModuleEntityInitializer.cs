using System;
using System.Collections.Generic;
using System.Linq;
using IM.Common;
using IM.Modules;
using IM.SaveSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IM
{
    [CreateAssetMenu(menuName = "Initialization/Module Entity Initializer")]
    public class ModuleEntityInitializer : SceneInitializer
    {
        [SerializeField] private List<ModuleEntityEntry> _toInitialize;
        
        public override void OnSceneLoaded(GameObject initializerGO, IGameObjectFactory factory)
        {
            foreach (ModuleEntityEntry entry in _toInitialize) InitializeEntry(entry,factory);
        }

        private void InitializeEntry(ModuleEntityEntry entry, IGameObjectFactory factory)
        {
            GameObject created = factory.Create(entry.EntityGameObject, false);
            created.transform.position = new Vector3(Random.Range(entry.Bounds.min.x, entry.Bounds.max.x),
                Random.Range(entry.Bounds.min.y, entry.Bounds.max.y));
            IModuleEntity entity = created.GetComponent<IModuleEntity>();
            IEnumerable<IExtensibleModule> modules = entry.ModulesGameObjects.Select(x => factory.Create(x,false).GetComponent<IExtensibleModule>());
            
            AddModulesToEntity(entity, modules);
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

        [Serializable]
        private class ModuleEntityEntry
        {
            public Bounds Bounds;
            public GameObject EntityGameObject;
            public List<GameObject> ModulesGameObjects;
        }
    }
}