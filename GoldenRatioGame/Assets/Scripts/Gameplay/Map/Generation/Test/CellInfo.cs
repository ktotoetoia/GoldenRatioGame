using System.Collections.Generic;

namespace IM.Map.Grid
{
    public class CellInfo
    {
        public IRoomPattern Pattern { get; }
        public IRoomFactory Factory { get; }
        public ISelectedRoomPattern SelectedPattern { get; private set; }

        public CellInfo(IRoomPattern pattern, IRoomFactory factory)
        {
            Pattern = pattern;
            Factory = factory;
        }

        public void Select(IEnumerable<IPortDefinition> validOptionals)
        {
            SelectedPattern = Pattern.Select(validOptionals);
        }
    }
}