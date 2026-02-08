using UnityEngine;

namespace IM.Visuals
{
    public interface IMemberValueProvider<T>
    {
        public Component TargetComponent { get; }
        public string MemberName { get; }

        SerializedValue GetC(T t);
    }
}