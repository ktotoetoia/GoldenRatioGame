using IM.Abilities;

namespace IM.WeaponSystem
{
    public interface IWeapon
    {
        IAbilityReadOnly Ability { get; }
    }
}