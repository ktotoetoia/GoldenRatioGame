using System;

namespace IM.Map
{
    public interface ITransitionPoint
    {
        bool IsOpen { get; set; }

        event Action Interacted;
    }
}