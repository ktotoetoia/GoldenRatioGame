using System;
using UnityEngine;

namespace IM.SaveSystem
{
    public abstract class ComponentSerializer<T> : IComponentSerializer where T : Component
    {
        public Type TargetType => typeof(T);
        public abstract object CaptureState(T component);
        public abstract void RestoreState(T component, object state,Func<string,GameObject> resolveDependency);

        object IComponentSerializer.CaptureState(Component component) => CaptureState((T)component);
        void IComponentSerializer.RestoreState(Component component, object state, Func<string, GameObject> resolveDependency) => RestoreState((T)component, state,resolveDependency);
    }
}