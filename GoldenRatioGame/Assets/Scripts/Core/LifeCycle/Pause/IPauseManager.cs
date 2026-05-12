namespace IM
{
    public interface IPauseManager
    {
        bool Paused { get; }
        
        void SetPaused(bool paused);
    }
}