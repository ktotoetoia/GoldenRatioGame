using System.Collections.Generic;
using IM.Graphs;

namespace IM.Modules
{
    public interface ICoreModule : IModule
    {
        public void OnStructureUpdated();
    }
}