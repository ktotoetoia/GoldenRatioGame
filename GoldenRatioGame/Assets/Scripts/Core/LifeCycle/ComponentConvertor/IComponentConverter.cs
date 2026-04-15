using System;

namespace IM.Modules
{
    public interface IComponentConverter
    {
        Type MutableType { get; }
        Type ReadOnlyType { get; }
        object CreateReadOnly();
        object ToReadOnly(object mutable);
        object ToMutable(object readOnly);
    }
}