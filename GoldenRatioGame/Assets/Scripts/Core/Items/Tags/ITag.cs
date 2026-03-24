namespace IM.Items
{
    public interface ITag
    {
        string TagName { get; }
        
        bool Matches(ITag other);
    }
}