using System;
using IM.Modules;
using UnityEngine;

namespace IM.UI
{
    public interface IGraphViewInteraction
    {
        Func<Vector3> GetPointerPosition { get; set; }
        Func<bool> ShouldTryQuickRemoveAtPointer { get; set; }
        Func<bool> ShouldTryQuickRemove { get; set; }
        Func<bool> ShouldUndo { get; set; }
        Func<bool> ShouldRedo { get; set; }
        
        void SetContext(IModuleEditingContext moduleEditingContext);
        void ClearContext();
    }
}