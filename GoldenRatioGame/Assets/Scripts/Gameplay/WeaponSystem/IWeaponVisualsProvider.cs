using UnityEngine.Pool;

namespace IM.WeaponSystem
{
    public interface IWeaponVisualsProvider
    {
        IObjectPool<IWeaponVisual> WeaponVisualsPool { get; }
    }
}