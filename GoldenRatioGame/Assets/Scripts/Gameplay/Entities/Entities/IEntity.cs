using UnityEngine;

namespace IM.Entities
{
    public interface IEntity : IPausable
    {
        GameObject GameObject { get; }
    }
}