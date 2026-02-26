namespace IM.Abilities
{
    public interface ICastInfo
    {
        bool Completed { get; }
    }

    public class CastInfo : ICastInfo
    {
        public bool Completed { get; }
        
        
    }
}