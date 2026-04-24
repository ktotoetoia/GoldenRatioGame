using System;
using IM.Transforms;

namespace IM.Visuals
{
    public interface IVisualObject : IDisposable, IOrderable, IHaveTransform
    {
        bool Visible { get; set; }
        int Layer { get; set; }
    }
}