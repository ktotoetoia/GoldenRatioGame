using System;

namespace IM.Visuals
{
    public interface IVisualObject : IDisposable
    {
        bool Visible { get; set; }
        int Order { get; set; }
        int Layer { get; set; }
    }
}