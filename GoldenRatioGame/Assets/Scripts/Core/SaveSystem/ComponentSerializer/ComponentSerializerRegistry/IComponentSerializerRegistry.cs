using System;

namespace IM.SaveSystem
{
    public interface IComponentSerializerRegistry
    {
        IComponentSerializer GetSerializerFor(Type type);
    }
}