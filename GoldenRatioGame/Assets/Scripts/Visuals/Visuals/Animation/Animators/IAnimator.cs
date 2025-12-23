namespace IM.Visuals
{
    public interface IAnimator
    {
        void SetBool(string propertyName, bool value);
        void SetTrigger(string propertyName);
        void SetFloat(string propertyName, float value);
        void SetInteger(string propertyName, int value);
    }
}