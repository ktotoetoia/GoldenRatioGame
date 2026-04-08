using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    public interface IGameObjectRoom : IRoom
    {
        IEnumerable<GameObject> GameObjects { get; }
        
        bool Add(GameObject gameObject);
        bool Remove(GameObject gameObject);
    }
}