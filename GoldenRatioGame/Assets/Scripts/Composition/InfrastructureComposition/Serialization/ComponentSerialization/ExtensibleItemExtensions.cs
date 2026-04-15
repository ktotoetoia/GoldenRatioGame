using IM.LifeCycle;

namespace IM.Modules
{
    internal static class ExtensibleItemExtensions
    {
        public static string GetModuleId(this IExtensibleItem item)
        {
            return item != null && item.Extensions.TryGet(out IIdentifiable identifiable)
                ? identifiable.Id
                : null;
        }
    }
}