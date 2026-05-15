using System;
using UnityEngine;

namespace IM.LifeCycle
{
    [Serializable]
    public class ValueInfo
    {
        [field: SerializeField] public string Name { get; set; }
        [field:SerializeField] public string Value { get; set; }

        public ValueInfo(string name,string value)
        {
            Name = name;
            Value = value;
        }
    }
}