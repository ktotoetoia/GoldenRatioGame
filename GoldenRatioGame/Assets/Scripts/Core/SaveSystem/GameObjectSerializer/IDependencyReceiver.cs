using System.Collections.Generic;
using UnityEngine;

namespace IM.SaveSystem
{
    public interface IDependencyReceiver
    {
        void InjectDependencies(IReadOnlyDictionary<string, GameObject> resolved);
    }
}