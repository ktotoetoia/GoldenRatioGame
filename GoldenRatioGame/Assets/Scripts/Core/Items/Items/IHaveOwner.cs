namespace IM.Items
{
    public interface IHaveOwner
    {
        object Owner { get; }
        
        bool SetOwner(object owner);
    }
}