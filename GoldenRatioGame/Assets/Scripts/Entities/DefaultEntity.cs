using UnityEngine;

namespace IM.Entities
{
    public class DefaultEntity : MonoBehaviour, IEntity
    {
        public GameObject GameObject => gameObject;
    }
}