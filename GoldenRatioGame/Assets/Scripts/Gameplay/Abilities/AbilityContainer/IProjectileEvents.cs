using System;
using UnityEngine;

namespace IM.Abilities
{
    public interface IProjectileEvents
    {
        event Action<GameObject> ProjectileGet;
        event Action<GameObject> ProjectileRelease;
    }
}