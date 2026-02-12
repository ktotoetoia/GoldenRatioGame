using IM.Modules;
using IM.UI;

namespace IM.Visuals.Graph
{
    public interface IModuleContextView
    {
        public void SetModuleContext(IModuleEditingContext moduleEditingContext);
        public void ClearModuleContext();
    }
}