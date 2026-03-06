using System;

namespace IM.Abilities
{
    public interface ITemporary
    {
        void Initialize(Action onFinished);
    }
}