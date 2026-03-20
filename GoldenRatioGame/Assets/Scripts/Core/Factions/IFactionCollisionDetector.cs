using System;
using UnityEngine;

namespace IM.Factions
{
    public interface IFactionCollisionDetector
    {
        event Action<GameObject> OnTriggerEnterEnemy;
        event Action<GameObject> OnTriggerEnterAlly;
        event Action<GameObject> OnTriggerEnterNone;
    }
}