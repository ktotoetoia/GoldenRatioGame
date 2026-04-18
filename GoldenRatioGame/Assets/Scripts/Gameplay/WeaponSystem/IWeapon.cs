using IM.Abilities;
using IM.LifeCycle;

namespace IM.WeaponSystem
{
    public interface IWeapon : IAbilityReadOnly, IRequireEntity
    {
        IWeaponVisualsProvider WeaponVisualsProvider { get; }
    }
}