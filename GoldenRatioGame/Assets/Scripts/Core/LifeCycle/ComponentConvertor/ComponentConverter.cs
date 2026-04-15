using System;

namespace IM.Modules
{
    public abstract class ComponentConverter<TMutable, TReadOnly> : IComponentConverter
    {
        public Type MutableType => typeof(TMutable);
        public Type ReadOnlyType => typeof(TReadOnly);

        public object CreateReadOnly() => CreateNewReadOnly();
        public object ToReadOnly(object mutable) => ConvertToReadOnly((TMutable)mutable);
        public object ToMutable(object readOnly) => ConvertToMutable((TReadOnly)readOnly);

        protected abstract TReadOnly CreateNewReadOnly();
        protected abstract TReadOnly ConvertToReadOnly(TMutable mutable);
        protected abstract TMutable ConvertToMutable(TReadOnly readOnly);
    }
}