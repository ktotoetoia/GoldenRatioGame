using System;
using IM.Entities;
using IM.Modules;
using IM.StateMachines;
using IM.Visuals.Graph;
using UnityEngine;

namespace Tests
{
    public class LegacyPlayerInput : MonoBehaviour, IRequirePlayerEntity
    {
        [SerializeField] private EntityModuleAbilityContextEditingViewer entityModuleAbilityContextEditingViewer;
        [SerializeField] private Camera _gameCamera;
        private IModuleEntity _moduleEntity;
        private IStateMachine _stateMachine;
        
        private void Update()
        {
            _stateMachine.Update();
            EditorInput();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }

        private void EditorInput()
        {
            if (Input.GetKeyDown(KeyCode.I)) entityModuleAbilityContextEditingViewer.SetEntity(_moduleEntity);
            if (Input.GetKeyDown(KeyCode.O)) entityModuleAbilityContextEditingViewer.ClearEntity();
        }

        public void SetPlayerEntity(IEntity playerEntity)
        {
            _moduleEntity = playerEntity as IModuleEntity ?? throw new NullReferenceException();
            _stateMachine = new LegacyInputStateMachineFactory(playerEntity,_gameCamera).Create();
        }
    }
}