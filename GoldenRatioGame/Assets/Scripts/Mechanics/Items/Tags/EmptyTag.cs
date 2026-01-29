namespace IM.Items
{
    public class EmptyTag : ITag
    {
        public string TagName => string.Empty;
        
        public bool Matches(ITag other)
        {
            return false;
        }
    }
}