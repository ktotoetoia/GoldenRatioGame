namespace IM.SaveSystem
{
    public interface IHaveComponentRegistry
    {
        public IComponentSerializerRegistry Registry { get; set; }
    }
}