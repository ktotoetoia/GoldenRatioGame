using System;

namespace IM.Items
{
    public interface ITag : IEquatable<ITag>
    {
        string TagName { get; }
    }
}