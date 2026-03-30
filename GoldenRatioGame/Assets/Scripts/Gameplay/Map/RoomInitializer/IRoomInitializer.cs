using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    public interface IRoomInitializer
    {
        IEnumerable<GameObject> Initialize(IRoom room);
    }
}