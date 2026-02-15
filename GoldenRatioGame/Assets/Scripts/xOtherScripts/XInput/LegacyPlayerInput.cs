using System;
using System.Collections.Generic;
using IM.Abilities;
using IM.Entities;
using IM.Modules;
using IM.StateMachines;
using IM.Movement;
using IM.UI;
using IM.Visuals.Graph;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Tests
{
    public class LegacyPlayerInput : MonoBehaviour, IRequirePlayerEntity
    {
        [SerializeField] private ModuleContextView moduleContext;
        private IModuleEntity _moduleEntity;
        private IMoveInVector _movement;
        private IStateMachine _stateMachine;
        private IKeyAbilityPool  _keyAbilityPool;
        
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
            if (Input.GetKeyDown(KeyCode.I)) moduleContext.SetModuleContext(_moduleEntity.ModuleEditingContext);
            if (Input.GetKeyDown(KeyCode.O)) moduleContext.ClearModuleContext();

            foreach (KeyValuePair<KeyCode, IAbility> f in _keyAbilityPool.KeyMap)
            {
                if (Input.GetKeyDown(f.Key))
                {
                    if (f.Value is IRequireAbilityUseContext requireAbilityUseContext)
                    {
                        requireAbilityUseContext.UpdateAbilityUseContext(new AbilityUseContext(Camera.main.ScreenToWorldPoint(Input.mousePosition) *Vector2.one,_moduleEntity.GameObject.transform.position));
                    }
                    
                    f.Value.TryUse();
                }
            }
        }

        public void SetPlayerEntity(IEntity playerEntity)
        {
            _moduleEntity = playerEntity as IModuleEntity ?? throw new NullReferenceException();
            
            _movement = _moduleEntity.GameObject.GetComponent<IMoveInVector>();
            _stateMachine = new StateMachine(new MovementState(_movement, () => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))));
            _keyAbilityPool = _moduleEntity.GameObject.GetComponent<IKeyAbilityPool>();
        }
    }
}