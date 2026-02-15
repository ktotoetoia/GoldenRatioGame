using System;

namespace IM.Abilities
{
    public interface IProjectile
    {
        void Initialize(Action onFinished);
    }
}