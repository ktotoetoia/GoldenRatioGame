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
            return other && TagName == other.TagName;
        }

        public virtual bool Equals(ITag other)
        {
            return other is LazyTag tag && Equals(tag);
        }
    }
}