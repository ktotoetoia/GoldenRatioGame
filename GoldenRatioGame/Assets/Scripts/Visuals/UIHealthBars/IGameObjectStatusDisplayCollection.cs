using System.Collections.Generic;
using UnityEngine;

namespace IM.UI
{
    public interface IGameObjectStatusDisplayCollection
    {
        IEnumerable<GameObject> Displayed { get; }
        
        void Add(GameObject go);
        void Remove(GameObject go);
    }
}