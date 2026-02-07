using System.Collections.Generic;

namespace IM.Visuals
{
    public class PortVisualObjectChange : IPortVisualObjectChange
    {
        private readonly HashSet<IPortVisualObject> _list = new();

        public IReadOnlyCollection<IPortVisualObject> Changed => _list;
        
        public void AddChanged(IPortVisualObject portVisualObject)
        {
            _list.Add(portVisualObject);
        }

        public void Clear()
        {
            _list.Clear();
        }
    }
}