using IM.Entities;
using IM.Graphs;

namespace IM.Modules
{
    public interface IModulesObserver
    {
        public void OnGraphStructureChanged(IGraphReadOnly graph, IEntity entity);
    }
}