using IM.Modules;
using UnityEngine.Pool;

namespace IM.Visuals
{
    public interface IModuleVisual : IExtension,IObjectPool<IModuleVisualObject>
    {
        
    }
}