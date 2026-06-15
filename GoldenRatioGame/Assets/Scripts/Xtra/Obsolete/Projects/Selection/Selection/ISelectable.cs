namespace IM.SelectionSystem
{
    public interface ISelectable
    {
        bool TryGetObject<T>(out T obj);
    }
}