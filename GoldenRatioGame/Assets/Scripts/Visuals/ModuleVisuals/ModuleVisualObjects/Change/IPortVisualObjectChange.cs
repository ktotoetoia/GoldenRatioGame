using System.Collections.Generic;

namespace IM.Visuals
{
    public interface IPortVisualObjectChange
    {
        IReadOnlyCollection<IPortVisualObject> Changed { get; }
        
        void AddChanged(IPortVisualObject portVisualObject);
        void Clear();
    }
}