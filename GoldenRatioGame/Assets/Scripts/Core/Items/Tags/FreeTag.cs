namespace IM.Items
{
    public class FreeTag : ITag
    {
        public string TagName => string.Empty;
        
        public bool Matches(ITag other)
        {
            return true;
        }
    }
}