namespace IM.Values
{
    public static class NumerableResourceExtensions
    {
        public static bool TryGet<T>(this INumerableResource<T> resource, int count)
        {
            if (resource.Has(count))
            {
                resource.Value -= count;
                
                return true;
            }
            
            return false;
        }

        public static bool Has<T>(this INumerableResource<T> resource, int count)
        {
            return resource.Value >= count;
        }
    }
}