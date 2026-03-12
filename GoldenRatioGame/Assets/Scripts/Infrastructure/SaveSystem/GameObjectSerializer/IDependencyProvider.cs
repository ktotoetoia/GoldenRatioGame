using System.Collections.Generic;

namespace IM.SaveSystem
{
    public interface IDependencyProvider
    {
        IEnumerable<string> GetDependencyIds();
    }
}