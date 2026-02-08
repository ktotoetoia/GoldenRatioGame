using System;
using IM.Items;
using UnityEngine;

namespace IM.Modules
{
    [Serializable]
    public class PortTag
    {
        [SerializeField] private LazyTag _tag;
        
        public ITag Tag => (ITag)_tag ?? new EmptyTag();
        [field: SerializeField]public PortDirection Direction { get; private set; }
    }
}