using IM.LifeCycle;
using UnityEngine;

namespace IM.Modules
{
    internal static class EntityExtension
    {
        public static string GetModuleId(this IEntity entity)
        {
            return entity != null && entity.GameObject.TryGetComponent(out IIdentifiable identifiable)
                ? identifiable.Id
                : null;
        }
    }
}