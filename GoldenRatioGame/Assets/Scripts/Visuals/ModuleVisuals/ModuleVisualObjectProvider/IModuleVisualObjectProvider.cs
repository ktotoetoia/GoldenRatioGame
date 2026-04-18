using IM.Modules;
using UnityEngine.Pool;

namespace IM.Visuals
{
    public interface IModuleVisualObjectProvider : IExtension
    {
        IObjectPool<IModuleVisualObject> EditorPool {get;}
        IObjectPool<IModuleVisualObject> GamePool { get; }
    }
}