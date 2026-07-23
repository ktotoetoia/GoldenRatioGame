using UnityEngine;

namespace IM.Map
{
    [CreateAssetMenu(menuName = "Map/Prop Info")]
    public class PropInfo : ScriptableObject, IPropInfo
    {
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public Vector2Int CellSize { get; private set; }
        [field: SerializeField] public float ClearanceRadius { get; private set; }
    }
}