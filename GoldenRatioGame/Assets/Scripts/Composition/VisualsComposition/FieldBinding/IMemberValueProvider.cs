using UnityEngine;

namespace IM.Visuals
{
    public interface IMemberValueProvider<in T>
    {
        public Component TargetComponent { get; }
        public string MemberName { get; }

        SerializedValue GetT(T t);
    }
}