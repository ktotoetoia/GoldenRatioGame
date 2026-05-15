using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IM.LifeCycle;
using UnityEngine;

namespace IM.SaveSystem
{
    [CreateAssetMenu(fileName = "GameInfoManager", menuName = "Game/Game Info Manager")]
    public class GameInfoControllerImplementation : GameInfoController
    {
        [SerializeField] private SceneLoadContext _gameLoadContext;
        [SerializeField] private SceneLoadContext _hubLoadContext;
        [SerializeField] private string _saveFolder = "Saves";
        [SerializeField] private string _saveFileName = "gameInfo";
        [SerializeField] private string _extensionName = "clnk";
        [SerializeField] private string _runExtensionName = "clnkrun";
        private readonly List<GameInfo> _gameInfos = new();
        private GameInfo _currentGameInfo;
        
        public override IEnumerable<GameInfo> GameInfos => _gameInfos;
        public override GameInfo CurrentGameInfo => _currentGameInfo;
        
        public override void Synchronize()
        {
            string savePath = GetSavePath();
            Debug.Log("Saving game info to: " + savePath);
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            _gameInfos.Clear();

            string searchPattern = $"*.{_extensionName}";
            string[] files = Directory.GetFiles(savePath, searchPattern);

            foreach (string file in files)
            {
                try
                {
                    string text = File.ReadAllText(file);
                    GameInfo gameInfo = JsonUtility.FromJson<GameInfo>(text);
                    
                    if (gameInfo != null)
                    {
                        _gameInfos.Add(gameInfo);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Could not load the file: {file}. Deviation: {e.Message}");
                }
            }
        }
        
        public override GameInfo CreateAtIndex(int index)
        {
            if (_gameInfos.Any(x => x.Index == index)) 
                throw new ArgumentException($"Index {index} already exists.");

            GameInfo gameInfo = new GameInfo(index);
            _gameInfos.Add(gameInfo);

            Save(gameInfo);
            
            return gameInfo;
        }

        public override void DeleteAtIndex(int index)
        {
            GameInfo gameInfo = GetByIndex(index);
            if (gameInfo == null) return;
            
            _gameInfos.Remove(gameInfo);
            
            string path = GetGameInfoPath(gameInfo);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public override GameInfo GetByIndex(int index)
        {
            return _gameInfos.FirstOrDefault(x => x.Index == index);
        }

        public override void StartSession(GameInfo gameInfo)
        {
            if (!_gameInfos.Contains(gameInfo)) throw new Exception("GameInfo not found");
            if (CurrentGameInfo != null) throw new Exception("Only one game info can be loaded at a time");
            
            _currentGameInfo = gameInfo;
        }

        public override void FinishSession()
        {
            _currentGameInfo = null;
        }

        public override void SaveAll()
        {
            foreach (GameInfo gameInfo in _gameInfos)
            {
                Save(gameInfo);
            }
        }

        public override void Save()
        {
            if (CurrentGameInfo == null) return;
            Save(CurrentGameInfo);
        }

        private void Save(GameInfo gameInfo)
        {
            Directory.CreateDirectory(GetSavePath());
            
            string savePath = GetGameInfoPath(gameInfo);
            string saveContent = JsonUtility.ToJson(gameInfo);
            
            File.WriteAllText(savePath, saveContent);
        }

        private string GetGameInfoPath(GameInfo gameInfo)
        {
            string fileName = $"{_saveFileName}{gameInfo.Index}.{_extensionName}";
            return Path.Combine(GetSavePath(), fileName);
        }

        private string GetSavePath()
        {
            return Path.Combine(Application.persistentDataPath, _saveFolder);
        }

        private void OnDisable()
        {
            _gameInfos.Clear();
            _currentGameInfo = null;
        }

        public override void ProgressTo(Location location)
        {
            throw new NotImplementedException();
        }
    }
}