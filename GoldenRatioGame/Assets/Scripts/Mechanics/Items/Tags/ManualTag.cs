using System;
using UnityEngine;

namespace IM.Items
{
    [CreateAssetMenu(menuName = "Tags/ManualTag")]
    public class ManualTag : LazyTag, IEquatable<ManualTag>
    {
        [SerializeField] private string _tagName;
        
        public override string TagName => _tagName;
        
        public bool Equals(ManualTag other)
        {
            return other && _tagName == other._tagName;
        }
    }
}