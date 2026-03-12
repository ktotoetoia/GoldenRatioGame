using System;

namespace IM.SaveSystem
{
    [Serializable]
    public class ComponentData
    {
        public string TypeAlias;
        public int ComponentIndex;
        public object SerializedState;
    }
}