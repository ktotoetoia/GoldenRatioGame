using System;

namespace IM.Modules
{
    public interface ICapabilitySnapshot
    {
        Type CapabilityType { get; }
        object Snapshot(object original);
    }
}