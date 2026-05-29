using IM.LifeCycle;
using UnityEngine;

namespace IM.EntityIntelligence
{
    public class MemoryContainer : MonoBehaviour, IMemoryContainer
    {
        private readonly TypeRegistry<IMemory> _typeRegistry = new();
        
        public ITypeRegistry<IMemory> Memories => _typeRegistry;
        
        public void Add(IMemory memory) => _typeRegistry.Add(memory);
        public void Remove(IMemory memory) => _typeRegistry.Remove(memory);
    }
}