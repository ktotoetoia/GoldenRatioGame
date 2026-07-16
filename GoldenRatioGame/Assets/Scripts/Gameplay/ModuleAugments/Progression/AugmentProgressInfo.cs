using System;
using UnityEngine;

namespace IM.Augments
{
    [Serializable]
    public struct AugmentProgressInfo
    {
        [field: SerializeField] public int Value { get; private set; }
        [field: SerializeField] public int CurrentIndex { get; private set; }
            
        public AugmentProgressInfo(int value, int currentIndex)
        {
            Value = value;
            CurrentIndex = currentIndex;
        }

        public AugmentProgressInfo Add(int value) => new(Value + value, CurrentIndex);
        public AugmentProgressInfo Next(int index) => new(index, CurrentIndex+1);
    }
}