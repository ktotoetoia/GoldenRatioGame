using IM.Abilities;
using IM.LifeCycle;
using IM.Values;
using UnityEngine;

namespace IM.WeaponSystem
{
    public class ContainerBasedWeapon : MonoBehaviour, IWeapon, ICastAbility,IRequireAbilityUseContext, IFocusPointProvider
    {
        private ICastAbility _castAbility;

        public string Name => _castAbility.Name;
        public string Description => _castAbility.Description;
        public ICooldownReadOnly Cooldown => _castAbility.Cooldown;
        public bool CanUse => _castAbility.CanUse;
        public IWeaponVisualsProvider WeaponVisualsProvider { get; private set; }
        public IEntity Entity { get; set; }

        private void Awake()
        {
            WeaponVisualsProvider = GetComponent<IWeaponVisualsProvider>();
            _castAbility = GetComponent<IAbilityContainer>().Ability as ICastAbility;
        }

        public bool TryCast(out ICastInfo info) => _castAbility.TryCast(out info);
        public void UpdateAbilityUseContext(UseContext context) => 
            (_castAbility as IRequireAbilityUseContext)?.UpdateAbilityUseContext(context);
        public float FocusTime => (_castAbility as IFocusPointProvider)?.FocusTime ?? 0;
        public Vector3 GetFocusPoint() => (_castAbility as IFocusPointProvider)?.GetFocusPoint() ?? default;
        public Vector3 GetFocusDirection() => (_castAbility as IFocusPointProvider)?.GetFocusDirection() ?? default;
    }
}