using System;
using System.Collections.Generic;

namespace IM.SaveSystem
{
    [Serializable] 
    public class SceneSaveFile { public List<GameObjectData> Objects = new(); public string Version = "1"; }
}