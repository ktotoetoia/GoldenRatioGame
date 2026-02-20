using IM.Modules;

namespace IM.Visuals
{
    public interface IRequireReferenceModuleVisualObject : IExtension
    {
        void SetReferenceModuleVisualObject(IModuleVisualObject moduleVisualObject);
    }
}