using System;

namespace IM.Common
{
    public interface ITemporary
    {
        void Initialize(Action onFinished);
    }
}