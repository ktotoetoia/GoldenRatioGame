namespace IM.Abilities
{
    public class CastInfo : ICastInfo
    {
        public bool Completed { get; set; }
        
        public CastInfo(bool completed = true)
        {
            Completed = completed;
        }
    }
}