namespace IM.SaveSystem
{
    public interface IRequireComponentSerializerContainer
    {
        public IComponentSerializerContainer Container { get; set; }
    }
}