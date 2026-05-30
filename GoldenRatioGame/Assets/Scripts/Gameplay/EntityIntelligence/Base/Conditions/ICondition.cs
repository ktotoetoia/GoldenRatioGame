namespace IM.EntityIntelligence
{
    public interface ICondition
    {
        void Start();
        void Finish();
        bool Check();
    }
}