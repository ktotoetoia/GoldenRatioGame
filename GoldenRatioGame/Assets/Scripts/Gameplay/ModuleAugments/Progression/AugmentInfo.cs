using System;
using UnityEngine;

namespace IM.Augments
{
    [Serializable]
    public struct AugmentInfo
    {
        [SerializeField] private AugmentFactory _factory;
        private IAugmentFactory _factory2;
        
        [field:SerializeField] public int RequiredProgress { get; set; }

        public IAugmentFactory Factory
        {
            get => _factory2 ?? _factory;
            set => _factory2 = value;
        }
        
        public AugmentInfo(int requiredProgress, IAugmentFactory factory)
        {
            RequiredProgress = requiredProgress;
            _factory2 = factory;
            _factory = null;
        }
    }
}