using IM.LifeCycle;

namespace IM.EntityIntelligence
{
    public interface IMemoryContainer
    {
        ITypeRegistry<IMemory> Memories { get; }
        
        void Add(IMemory memory);
        void Remove(IMemory memory);
    }
}