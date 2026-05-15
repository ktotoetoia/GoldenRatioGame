using System;
using UnityEngine;

namespace IM.UI
{
    [Serializable]
    public class DocumentTransitionContext
    {
        [SerializeField] private int _fromIndex = 0;
        [SerializeField] private int _toIndex = 1;
        [SerializeField] private string _transitionButtonName = "TransitionButton";
        [SerializeField] private bool _isOverlay;
        
        public int FromIndex => _fromIndex;
        public int ToIndex => _toIndex;
        public string TransitionButtonName => _transitionButtonName;
        public bool IsOverlay => _isOverlay;
    }
}