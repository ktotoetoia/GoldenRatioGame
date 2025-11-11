using System;
using UnityEngine;

namespace IM.Modules
{
    [CreateAssetMenu(menuName = "Tags/LazyTag")]
    public class LazyTag : ScriptableObject, ITag, IEquatable<LazyTag>
    {
        public virtual string TagName => name;

        public virtual bool Equals(LazyTag other)
        {
            if (!other) return false;
            if (ReferenceEquals(this, other)) return true;

            return TagName == other.TagName;
        }

        public virtual bool Equals(ITag other)
        {
            if (other is LazyTag tag) return Equals(tag);
            return false;
        }
    }
}