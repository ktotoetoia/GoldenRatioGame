using System;
using IM.Graphs;

namespace IM.Visuals
{
    public interface IModuleGraphStructureUpdater
    {
        void OnPortTransformChanged(IPort port);
    }
}