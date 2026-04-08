using System.Collections.Generic;
using IM.WeaponSystem;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class WeaponCommandObserverFactoryMono : MonoBehaviour, ICommandObserverAddFactory,
        ICommandObserverRemoveFactory
    {
        [SerializeField] private GameObject _weaponPoolSource;
        private IWeaponPoolDraftContainer _draftContainer;
        private CollectionCommandObserverFactory<IWeapon>  _commandObserverFactory;

        private void Awake()
        {
            _draftContainer = _weaponPoolSource.GetComponent<IWeaponPoolDraftContainer>();
            _commandObserverFactory = new CollectionCommandObserverFactory<IWeapon>((x,y) => GetModule(x), (x,y,z) => GetModule(x), _draftContainer.GetEditableDraft());
        }

        private IWeapon GetModule(IModule module) => (module as IExtensibleModule)?.Extensions?.Get<IWeaponExtension>()?.Weapon;
        public ICommandObserver Create(IModule param1, ICollection<IModule> param2) => _commandObserverFactory.Create(param1, param2);
        public ICommandObserver Create(IModule param1, ICollection<IModule> param2, ICollection<IConnection> param3) => _commandObserverFactory.Create(param1, param2, param3);
    }
}