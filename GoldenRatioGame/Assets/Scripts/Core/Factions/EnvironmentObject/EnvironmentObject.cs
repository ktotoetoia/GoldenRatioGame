using UnityEngine;

namespace IM.Factions
{
    public class EnvironmentObject : MonoBehaviour, IEnvironmentObject
    {
        [field: SerializeField] public bool CanCollide { get; private set; } = true;
    }
}