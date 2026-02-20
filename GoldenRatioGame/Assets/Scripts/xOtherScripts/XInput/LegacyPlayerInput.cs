using System;
using System.Collections.Generic;
using IM.Abilities;
using IM.Entities;
using IM.Modules;
using IM.StateMachines;
using IM.Movement;
using IM.Visuals.Graph;
using UnityEngine;

namespace Tests
{
    public class LegacyPlayerInput : MonoBehaviour, IRequirePlayerEntity
    {
        [SerializeField] private ModuleContextView _moduleContextView;
        [SerializeField] private Camera _gameCamera;
        private IModuleEntity _moduleEntity;
        private IMoveInVector _movement;
        private IStateMachine _stateMachine;
        private IAbilityUser<IKeyAbilityPool> _abilityUser;
        
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
            if (Input.GetKeyDown(KeyCode.I)) _moduleContextView.SetModuleContext(_moduleEntity.ModuleEditingContext);
            if (Input.GetKeyDown(KeyCode.O)) _moduleContextView.ClearModuleContext();
            
            Vector3 mousePosition = _gameCamera.ScreenToWorldPoint(Input.mousePosition) * Vector2.one;
            AbilityUseContext useContext = new AbilityUseContext(mousePosition, _moduleEntity.GameObject.transform.position);
            
            foreach (KeyValuePair<KeyCode, IAbility> f in _abilityUser.AbilityPool.KeyMap)
            {
                if (Input.GetKeyDown(f.Key))
                {
                    _abilityUser.UseAbility(f.Value,useContext);
                }
            }
        }

        public void SetPlayerEntity(IEntity playerEntity)
        {
            _moduleEntity = playerEntity as IModuleEntity ?? throw new NullReferenceException();
            
            _movement = _moduleEntity.GameObject.GetComponent<IMoveInVector>();
            _stateMachine = new StateMachine(new MovementState(_movement, () => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))));
            _abilityUser = _moduleEntity.GameObject.GetComponent<IAbilityUser<IKeyAbilityPool>>();
        }
    }
}