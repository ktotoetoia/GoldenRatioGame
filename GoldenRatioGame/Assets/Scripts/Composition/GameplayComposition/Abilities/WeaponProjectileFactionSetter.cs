using IM.Abilities;
using IM.Factions;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Modules
{
    public class WeaponProjectileFactionSetter : MonoBehaviour
    {
        private IProjectileEvents _projectileEvents;
        private IRequireEntity _entitySource;
        
        private void Awake()
        {
            _projectileEvents = GetComponent<IProjectileEvents>();
            _entitySource = GetComponent<IRequireEntity>();

            _projectileEvents.ProjectileGet += x =>
            {
                if (_entitySource is { Entity: not null } &&
                    _entitySource.Entity.GameObject.TryGetComponent(out IFactionMemberReadOnly factionMember) &&
                    x.gameObject.TryGetComponent(out IRequireFactionMember require))
                {
                    require.FactionMemberReadOnly = factionMember;
                } 
            };
        }
    }
}