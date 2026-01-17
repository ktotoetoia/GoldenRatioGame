using System;

namespace IM.Visuals
{
    public interface IVisualObject : IDisposable
    {
        bool Visibility { get; set; }
    }
}