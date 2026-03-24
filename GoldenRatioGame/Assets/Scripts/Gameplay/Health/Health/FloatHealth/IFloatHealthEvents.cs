using System;

namespace IM.Health
{
    public interface IFloatHealthEvents
    {
        event Action<float> OnHealthChanged;
    }
}