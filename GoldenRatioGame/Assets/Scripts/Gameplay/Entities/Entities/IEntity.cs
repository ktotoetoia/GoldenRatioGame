using UnityEngine;

namespace IM.Entities
{
    public interface IEntity
    {
        GameObject GameObject { get; }
        void Destroy();
    }
}