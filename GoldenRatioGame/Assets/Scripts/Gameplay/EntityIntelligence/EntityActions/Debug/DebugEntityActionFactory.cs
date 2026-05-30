using System;
using UnityEngine;

namespace IM.EntityIntelligence
{
    [Serializable]
    public class DebugEntityActionFactory : IEntityActionFactory
    {
        [SerializeField] private bool _printOnEnter = true;
        [SerializeField] private bool _printOnExit = false;
        [SerializeField] private bool _printOnUpdate = false;
        [SerializeField] private bool _printOnFixedUpdate = false;
        
        public IEntityAction Create(GameObject param1)
        {
            return new DebugEntityAction(_printOnEnter, _printOnExit, _printOnUpdate, _printOnFixedUpdate)
            {
                Text = param1.name
            };
        }
    }
}