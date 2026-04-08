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
        private CollectionCommandObserverFactory<IAbilityReadOnly>  _commandObserverFactory;

        private void Awake()
        {
            _abilityPool = _abilityPoolSource.GetComponent<IAbilityPoolDraftContainer>();
            _commandObserverFactory = new CollectionCommandObserverFactory<IAbilityReadOnly>((x,y) => GetModule(x), (x,y,z) => GetModule(x), _abilityPool.GetEditableDraft());
        }

        private IAbilityReadOnly GetModule(IModule module) => (module as IExtensibleModule)?.Extensions?.Get<IAbilityExtension>()?.Ability;
        public ICommandObserver Create(IModule param1, ICollection<IModule> param2) => _commandObserverFactory.Create(param1, param2);
        public ICommandObserver Create(IModule param1, ICollection<IModule> param2, ICollection<IConnection> param3) => _commandObserverFactory.Create(param1, param2, param3);
    }
}