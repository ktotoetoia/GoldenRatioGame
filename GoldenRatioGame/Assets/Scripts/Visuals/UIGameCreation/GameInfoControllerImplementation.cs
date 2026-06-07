using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IM.LifeCycle;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IM.SaveSystem
{
    [CreateAssetMenu(fileName = "GameInfoManager", menuName = "Game/Game Info Manager")]
    public class GameInfoControllerImplementation : GameInfoController
    {
        [Header("Scene Layout Contexts")]
        [SerializeField] private SceneLoadContext _gameLoadContext;
        [SerializeField] private SceneLoadContext _hubLoadContext;

        [Header("File Settings")]
        [SerializeField] private string _saveFolder = "Saves";
        [SerializeField] private string _saveFileName = "gameInfo";
        [SerializeField] private string _extensionName = "clnk";
        [SerializeField] private string _runExtensionName = "clnkrun";
        [SerializeField] private string _hubExtensionName = "clnkhub";

        private readonly List<GameInfo> _gameInfos = new();
        private GameInfo _currentGameInfo;
        
        public override IEnumerable<GameInfo> GameInfos => _gameInfos;
        public override GameInfo CurrentGameInfo => _currentGameInfo;
        
        public override void Synchronize()
        {
            _currentGameInfo = null;
            string savePath = GetSavePath();
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            _gameInfos.Clear();
            string[] files = Directory.GetFiles(savePath, $"*.{_extensionName}");

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

            string metaPath = GetGameInfoPath(gameInfo);
            if (File.Exists(metaPath)) File.Delete(metaPath);

            string runPath = GetSpecificSavePath(gameInfo, Location.Game);
            if (File.Exists(runPath)) File.Delete(runPath);

            string hubPath = GetSpecificSavePath(gameInfo, Location.Hub);
            if (File.Exists(hubPath)) File.Delete(hubPath);
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

            string runPath = GetSpecificSavePath(gameInfo, Location.Game);
            string hubPath = GetSpecificSavePath(gameInfo, Location.Hub);

            if (File.Exists(runPath))
            {
                LoadLocation(Location.Game, runPath);
            }
            else if (File.Exists(hubPath))
            {
                LoadLocation(Location.Hub, hubPath);
            }
            else
            {
                LoadLocation(Location.Hub, null);
            }
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

            var bootstrapper = FindAnyObjectByType<SceneBootstrapper>();
            if (bootstrapper != null)
            {
                string activeSavePath = GetSpecificSavePath(CurrentGameInfo, CurrentGameInfo.Location);
                string sceneData = bootstrapper.GetSaved();
                File.WriteAllText(activeSavePath, sceneData);
            }
        }

        public override void ProgressTo(Location location)
        {
            if (_currentGameInfo == null || _currentGameInfo.Location == location) return;

            string oldLocationPath = GetSpecificSavePath(_currentGameInfo, _currentGameInfo.Location);
            if (File.Exists(oldLocationPath))
            {
                File.Delete(oldLocationPath);
            }

            LoadLocation(location, null);
        }

        private void LoadLocation(Location location, string savePath)
        {
            _currentGameInfo.Location = location;
            Save(_currentGameInfo);

            SceneLoadContext context = (location == Location.Game) ? _gameLoadContext : _hubLoadContext;
            
            context.FullSceneLoadPath = savePath ?? string.Empty;
            context.SceneLoadType = string.IsNullOrEmpty(savePath) ? SceneLoadType.NewScene : SceneLoadType.LoadExisting;

            SceneManager.LoadScene(context.SceneIndex);
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
            return Path.Combine(GetSavePath(), $"{_saveFileName}{gameInfo.Index}.{_extensionName}");
        }

        private string GetSpecificSavePath(GameInfo gameInfo, Location location)
        {
            string extension = (location == Location.Game) ? _runExtensionName : _hubExtensionName;
            return Path.Combine(GetSavePath(), $"{_saveFileName}{gameInfo.Index}.{extension}");
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
    }
}