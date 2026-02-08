using System;
using IM.Abilities;
using IM.Entities;
using IM.Modules;
using IM.StateMachines;
using IM.Movement;
using IM.UI;
using IM.Visuals.Graph;
using UnityEngine;

namespace Tests
{
    public class LegacyPlayerInput : MonoBehaviour, IRequirePlayerEntity
    {
        [SerializeField] private ModuleGraphEditorView _moduleGraphEditor;
        private PreferredKeyboardBindingsAbilityUser _abilityUser;
        private IModuleEntity _moduleEntity;
        private IMoveInVector _movement;
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
            if (Input.GetKeyDown(KeyCode.Q)) _moduleGraphEditor.SetModuleEntity(_moduleEntity);
            if (Input.GetKeyDown(KeyCode.E)) _moduleGraphEditor.ClearModuleEntity();
        }

        public void SetPlayerEntity(IEntity playerEntity)
        {
            _moduleEntity = playerEntity as IModuleEntity ?? throw new NullReferenceException();
            
            _movement = _moduleEntity.GameObject.GetComponent<IMoveInVector>();
            _stateMachine = new StateMachine(new MovementState(_movement, () => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))));
            //_abilityUser = new PreferredKeyboardBindingsAbilityUser(_moduleEntity.AbilityPool);
        }
    }
}