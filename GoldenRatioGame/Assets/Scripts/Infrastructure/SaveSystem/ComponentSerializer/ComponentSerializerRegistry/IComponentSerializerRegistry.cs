using System;

namespace IM.SaveSystem
{
    public interface IComponentSerializerContainer
    {
        IComponentSerializer GetSerializerFor(Type type);
    }
}