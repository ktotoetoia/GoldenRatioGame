using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.LifeCycle
{
    [Serializable]
    public class GameInfo 
    {
        [field:SerializeField] public Location Location { get; set; }
        [field:SerializeField] public List<string> AvailableAssets { get; private set; } 
        [field:SerializeField] public List<ValueInfo> ValueInfos { get; private set; }
        [field:SerializeField] public int Index { get; set; }

        public GameInfo(int index) : this(Location.Hub,new List<string>(),new List<ValueInfo>(),index)
        {
            
        }
        
        public GameInfo(Location location,  List<string> availableAssets, List<ValueInfo> valueInfos,int index)
        {
            Location = location;
            AvailableAssets = availableAssets;
            ValueInfos = valueInfos;
            Index = index;
        }
    }
}