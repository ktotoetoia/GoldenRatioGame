using System;
using System.Collections.Generic;
using IM.Abilities;
using IM.Graphs;
using IM.Modules;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    public class AbilityPoolView : MonoBehaviour
    {
        private UIDocument _document;
        private AbilityPoolReadonlyContainer _container;
        private IModuleEntity _moduleEntity;
        private AbilityPoolDraft _abilityPoolDraft;
        private ModuleGraphSnapshotDiffer _differ;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _container = _document.rootVisualElement.Q<AbilityPoolReadonlyContainer>();
            _document.rootVisualElement.visible = false;
            _differ = new ModuleGraphSnapshotDiffer()
            {
                OnModuleAdded = x =>
                {
                    if (x is IExtensibleModule extensibleModule &&
                        extensibleModule.Extensions.TryGetAll(out IEnumerable<IAbilityExtension> abilityExtensions))
                    {
                        foreach (IAbilityExtension abilityExtension in abilityExtensions)
                        {
                            _abilityPoolDraft.AddAbility(abilityExtension.Ability);
                        }
                    }
                },

                OnModuleRemoved = x =>
                {
                    if (x is IExtensibleModule extensibleModule &&
                        extensibleModule.Extensions.TryGetAll(out IEnumerable<IAbilityExtension> abilityExtensions))
                    {
                        foreach (IAbilityExtension abilityExtension in abilityExtensions)
                        {
                            _abilityPoolDraft.RemoveAbility(abilityExtension.Ability);
                        } 
                    }
                },
            };
        }

        public void SetEntity(IModuleEntity moduleEntity)
        {
            if (_moduleEntity != null) throw new ArgumentException("Module Entity is already set");
            
            _moduleEntity = moduleEntity;
            
            if (moduleEntity.GameObject.TryGetComponent(out IAbilityPool abilityPool))
            {
                _abilityPoolDraft = new AbilityPoolDraft(abilityPool);
                
                _container.SetAbilityPool(_abilityPoolDraft);
            }
            
            _document.rootVisualElement.visible = true;
        }

        private void Update()
        {
            if(_moduleEntity == null) return;
            
            _differ.OnSnapshotChanged(_moduleEntity.ModuleEditingContext.GraphEditor.Snapshot);
        }

        public void ClearEntity()
        {
            if (_moduleEntity == null) return;
            
            _differ.OnSnapshotChanged(_moduleEntity.ModuleEditingContext.GraphEditor.Snapshot);
            
            if (_abilityPoolDraft != null)
            {
                _container.ClearAbilityPool();
                _abilityPoolDraft.Commit();
                _abilityPoolDraft = null;
            }
            
            _moduleEntity = null;
            _document.rootVisualElement.visible = false;
        }
    }
}