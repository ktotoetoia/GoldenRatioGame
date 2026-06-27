using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    [Serializable]
    public class ModuleEntityEntry : EntityEntry
    {
        [field:SerializeField] public List<GameObject> ModulesGameObjects{ get; private set; }
    }
}