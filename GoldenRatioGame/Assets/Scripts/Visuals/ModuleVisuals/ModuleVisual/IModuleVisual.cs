using IM.Modules;
using UnityEngine.Pool;

namespace IM.Visuals
{
    public interface IModuleVisual : IExtension
    {
        IObjectPool<IModuleVisualObject> EditorPool {get;}
        IObjectPool<IAnimatedModuleVisualObject> GamePool { get; }
    }
}