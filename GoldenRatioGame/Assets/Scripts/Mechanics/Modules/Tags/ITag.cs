using System;

namespace IM.Modules
{
    public interface ITag : IEquatable<ITag>
    {
        string TagName { get; }
    }
}