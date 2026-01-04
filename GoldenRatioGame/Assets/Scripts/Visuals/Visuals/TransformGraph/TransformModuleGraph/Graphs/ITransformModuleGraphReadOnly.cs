using System.Collections.Generic;
using IM.Graphs;

namespace IM.Visuals
{
    public interface ITransformModuleGraphReadOnly : IModuleGraphReadOnly
    {
        ITransform Transform { get; }
        new IEnumerable<ITransformModule> Modules { get; }
        new IEnumerable<ITransformConnection> Connections { get; }
    }
}