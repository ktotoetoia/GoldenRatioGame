using System;
using UnityEngine;

namespace IM.SaveSystem
{
    public interface IComponentSerializer
    {
        Type TargetType { get; }
        object CaptureState(Component component);
        void RestoreState(Component component, object state);
    }
}