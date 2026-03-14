using System;
using System.Collections.Generic;

namespace IM.SaveSystem
{
    [Serializable]
    public class GameObjectData
    {
        public string Id;
        public string PrefabId;
        public string ScenePath;
        public List<ComponentData> Components = new();
        public List<string> DependenciesIDS = new();
    }
}