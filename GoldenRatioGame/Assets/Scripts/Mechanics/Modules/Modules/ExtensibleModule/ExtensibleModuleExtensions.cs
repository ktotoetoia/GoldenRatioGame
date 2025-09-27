namespace IM.Modules
{
    public static class ExtensibleModuleExtensions
    {
        public static bool TryGetExtension<T>(this IExtensibleModule module, out T component)
        {
            if (module == null)
                throw new System.ArgumentException(nameof(module));

            component = module.GetExtension<T>();
            return component != null;
        }
    }
}