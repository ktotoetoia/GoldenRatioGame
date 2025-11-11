using System;
using UnityEngine;

namespace IM.Modules
{
    [CreateAssetMenu(menuName = "Tags/Tag")]
    public class ManualTag : LazyTag, IEquatable<ManualTag>
    {
        [SerializeField] private string _tagName;
        
        public override string TagName => _tagName;
        
        public bool Equals(ManualTag other)
        {
            if (!other) return false;
            if (ReferenceEquals(this, other)) return true;

            return _tagName == other._tagName;
        }
    }
}