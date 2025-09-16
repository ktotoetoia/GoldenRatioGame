namespace IM.Values
{
    public interface INumerableResource<out TResource> : ICappedValue<int> 
    {
        TResource Resource { get; }
    }
}