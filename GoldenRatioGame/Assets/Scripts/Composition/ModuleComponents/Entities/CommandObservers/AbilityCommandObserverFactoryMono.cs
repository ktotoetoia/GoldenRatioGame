using System.Collections.Generic;
using IM.Abilities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class AbilityCommandObserverFactoryMono : MonoBehaviour, ICommandObserverAddFactory, ICommandObserverRemoveFactory
    {
        [SerializeField] private GameObject _abilityPoolSource;
        private IAbilityPoolDraftContainer _abilityPool;

        private void Awake()
        {
            _abilityPool = _abilityPoolSource.GetComponent<IAbilityPoolDraftContainer>();
        }

        public ICommandObserver Create(IModule param1, ICollection<IModule> param2)
        {
            if (param1 is not IExtensibleModule extensibleModule ||
                !extensibleModule.Extensions.TryGet(out IAbilityExtension abilityExtension))
            {
                return new EmptyCommandObserver();
            }
            
            return new AbilityCommandObserver(_abilityPool.EditDraft(),abilityExtension.Ability);
        }

        public ICommandObserver Create(IModule param1, ICollection<IModule> param2, ICollection<IConnection> param3)
        {
            if (param1 is not IExtensibleModule extensibleModule ||
                !extensibleModule.Extensions.TryGet(out IAbilityExtension abilityExtension))
            {
                return new EmptyCommandObserver();
            }
            
            return new AbilityCommandObserver(_abilityPool.EditDraft(),abilityExtension.Ability);
        }
    }
}