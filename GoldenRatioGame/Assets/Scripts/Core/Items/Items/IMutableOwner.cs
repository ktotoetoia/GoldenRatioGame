namespace IM.Items
{
    public interface IMutableOwner : IHaveOwner
    {
        bool SetOwner(object owner);
    }
}