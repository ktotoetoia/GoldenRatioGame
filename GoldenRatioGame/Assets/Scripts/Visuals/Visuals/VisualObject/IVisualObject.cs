using System;

namespace IM.Visuals
{
    public interface IVisualObject : IDisposable, IOrderable
    {
        bool Visible { get; set; }
        int Layer { get; set; }
    }
}