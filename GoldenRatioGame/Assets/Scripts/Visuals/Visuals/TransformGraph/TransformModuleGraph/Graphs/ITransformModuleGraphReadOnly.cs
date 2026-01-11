using System.Collections.Generic;
using IM.Graphs;
using IM.Transforms;

namespace IM.Visuals
{
    public interface ITransformModuleGraphReadOnly : IModuleGraphReadOnly
    {
        ITransform Transform { get; }
        new IEnumerable<ITransformModule> Modules { get; }
        new IEnumerable<ITransformConnection> Connections { get; }
    }
}