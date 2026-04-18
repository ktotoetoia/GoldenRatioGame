using IM.Transforms;
using IM.Visuals;

namespace IM.WeaponSystem
{
    public interface IWeaponVisual : IVisualObject
    {
        ITransform Transform { get; }
    }
}