using System.Collections.Generic;
using IM.LifeCycle;
using UnityEngine;

namespace IM.SaveSystem
{
    public abstract class GameInfoController : ScriptableObject
    {
        public abstract IEnumerable<GameInfo> GameInfos { get; }
        public abstract GameInfo CurrentGameInfo { get; }
        public abstract void Synchronize();
        public abstract GameInfo CreateAtIndex(int index);
        public abstract void DeleteAtIndex(int index);
        public abstract GameInfo GetByIndex(int index);
        public abstract void StartSession(GameInfo gameInfo);
        public abstract void FinishSession();
        public abstract void Save();
        public abstract void SaveAll();
        public abstract void ProgressTo(Location location);
    }
}