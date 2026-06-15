using System;
using UnityEngine;

namespace IM.Values
{
    public readonly struct UseContext
    {
        private readonly Func<Vector3> _getTargetWorldPosition;
        private readonly Func<Vector3> _getAnchorPosition;
        private readonly Func<Vector3> _getEntityPosition;
        
        public Vector3 InitialTargetWorldPosition { get; }
        public Vector3 CurrentTargetWorldPosition => _getTargetWorldPosition?.Invoke() ?? Vector3.zero;
        
        public UseContext(Func<Vector3> targetWorldPosition, Func<Vector3> entityPosition) : this(targetWorldPosition,entityPosition,entityPosition)
        {
            
        }
        
        public UseContext(Func<Vector3> targetWorldPosition, Func<Vector3> entityPosition, Func<Vector3> anchorPosition)
        {
            _getTargetWorldPosition = targetWorldPosition;
            _getEntityPosition = entityPosition;
            _getAnchorPosition = anchorPosition;
            InitialTargetWorldPosition = _getTargetWorldPosition?.Invoke() ?? Vector3.zero;
        }
        
        public Vector3 GetDirection() => CurrentTargetWorldPosition - GetEntityPosition();
        public Vector3 GetInitialDirection() => InitialTargetWorldPosition - GetEntityPosition();
        public Vector3 GetAnchorPosition() => _getAnchorPosition?.Invoke() ?? Vector3.zero;
        public Vector3 GetEntityPosition() => _getEntityPosition?.Invoke() ?? Vector3.zero;
    }
}