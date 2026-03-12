using System;
using System.Collections.Generic;

namespace IM.SaveSystem
{
    [Serializable]
    public class SaveFile
    {
        public List<GameObjectData> Objects = new();
        public long SavedAtTicks;
    }
}