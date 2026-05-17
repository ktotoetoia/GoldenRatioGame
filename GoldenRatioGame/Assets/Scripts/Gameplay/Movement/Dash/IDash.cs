using UnityEngine;

namespace IM.Movement
{
    public interface IDash
    {
        bool IsDashing { get; }
        void Trigger(Vector3 direction);
        void ForceStop();
    }
}