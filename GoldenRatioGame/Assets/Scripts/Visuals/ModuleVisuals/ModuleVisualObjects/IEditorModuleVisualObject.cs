using UnityEngine;

namespace IM.Visuals
{
    public interface IEditorModuleVisualObject : IModuleVisualObject
    {
        Bounds DefaultEditorLocalBounds { get; }
        Bounds EditorBounds { get; }
    }
}