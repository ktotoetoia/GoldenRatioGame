namespace IM.LifeCycle
{
    public class DefaultValidator<T> : IValidator<T>
    {
        public bool Value { get; set; } = true;
        public bool IsValid(T obj) => Value;
        public bool TryFix(T obj) => Value;
    }
}