using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IM.LifeCycle;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.SaveSystem
{
    public class SaveMenuController : MonoBehaviour
    {
        [SerializeField] private GameInfoController _gameInfoController;
        [SerializeField] private string _containerName;
        [SerializeField] private int _totalSlots = 3;
        private UIDocument _document;
 
        private SaveSlotsContainer<GameInfo> _container;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _gameInfoController.Synchronize();

            List<GameInfo> existingSaves = new List<GameInfo>();

            for (int i = 0; i < _totalSlots; i++)
            {
                existingSaves.Add(_gameInfoController.GetByIndex(i));
            }
            
            _container = new SaveSlotsContainer<GameInfo>(_totalSlots, existingSaves, new GameInfoFactory());
 
            _container.OnCreateRequested += OnCreateRequested;
            _container.OnLoadRequested += OnLoadRequested;
            
            _document.rootVisualElement.Q<VisualElement>(_containerName).Add(_container);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                _gameInfoController.SaveAll();
            }
        }

        private void OnCreateRequested(int slotIndex)
        {;
            _container.RefreshSlot(slotIndex, 
                _gameInfoController.CreateAtIndex(slotIndex));
            
            Debug.Log($"Creating new game in slot {slotIndex}");
        }
 
        private void OnLoadRequested(int slotIndex)
        {
            Debug.Log($"Loading slot {slotIndex}");
        }
    }
}