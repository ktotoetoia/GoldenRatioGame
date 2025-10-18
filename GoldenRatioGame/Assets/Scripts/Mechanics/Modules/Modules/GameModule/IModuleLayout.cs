using System.Collections.Generic;

namespace IM.Modules
{
    public interface IModuleLayout
    {
        IEnumerable<IPortSettings> PortSettings { get; }
    }
}