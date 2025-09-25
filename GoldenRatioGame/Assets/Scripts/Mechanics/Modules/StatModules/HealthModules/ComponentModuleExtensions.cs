namespace IM.Modules
{
    public static class ComponentModuleExtensions
    {
        public static bool TryGetComponent<T>(this IComponentModule module, out T component)
        {
            if (module == null)
                throw new System.ArgumentException(nameof(module));

            component = module.GetComponent<T>();
            return component != null;
        }
    }
}