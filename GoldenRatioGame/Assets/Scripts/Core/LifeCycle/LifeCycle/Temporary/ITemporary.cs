using System;

namespace IM.LifeCycle
{
    public interface ITemporary
    {
        void Initialize(Action onFinished);
    }
}