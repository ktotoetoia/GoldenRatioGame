namespace IM.Items
{
    public class EmptyTag : ITag
    {
        public string TagName => string.Empty;
        
        public bool Equals(ITag other)
        {
            return false;
        }
    }
}