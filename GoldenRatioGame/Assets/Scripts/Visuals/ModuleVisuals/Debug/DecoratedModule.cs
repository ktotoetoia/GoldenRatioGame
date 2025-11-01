using IM.Modules;
using UnityEngine;

namespace IM.ModuleGraphGizmosDebug
{
    public class DecoratedModule
    {
        public IGameModule Module { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }

        public DecoratedModule(IGameModule module, Vector3 position, Quaternion rotation)
        {
            Module = module;
            Position = position;
            Rotation = rotation;
        }
        
        public Vector3 GetPortWorldPosition(IPortLayout layout)
            => Position + Rotation * layout.RelativePosition;

        public Vector3 GetPortWorldNormal(IPortLayout layout)
            => (Rotation * layout.Normal).normalized;
    }
}