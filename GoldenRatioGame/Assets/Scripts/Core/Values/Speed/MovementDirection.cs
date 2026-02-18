namespace IM.Values
{
    [System.Flags]
    public enum MovementDirection
    {
        None  = 0,
        Left  = 1 << 0,
        Right = 1 << 1,
        Up    = 1 << 2,
        Down  = 1 << 3,
    }
}