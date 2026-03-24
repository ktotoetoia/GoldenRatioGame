using System.Collections.Generic;
using IM.Abilities;
using IM.Entities;
using IM.Modules;
using IM.UI;
using UnityEngine;

namespace IM.Inputs
{
    public class LegacyPlayerInput : MonoBehaviour, IRequirePlayerEntity
    {
        [SerializeField] private EntityModuleAbilityContextEditingViewer _entityModuleAbilityContextEditingViewer;
        [SerializeField] private GraphViewInteraction _graphViewInteraction;
        [SerializeField] private Camera _gameCamera;
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private List<KeyCode> _abilityKeys =  new ()
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Q,
            KeyCode.E
        };
        
        private IModuleEntity _moduleEntity;
        
        private void Update()
        {
            EditorInput();
        }
        
        private void EditorInput()
        {
            if (Input.GetKeyDown(KeyCode.I)) _entityModuleAbilityContextEditingViewer.SetEntity(_moduleEntity);
            if (Input.GetKeyDown(KeyCode.O)) _entityModuleAbilityContextEditingViewer.ForceClearEntity();
        }

        public void SetPlayerEntity(IEntity playerEntity)
        {
            _moduleEntity = playerEntity as IModuleEntity;
            PlayerStateMachine playerStateMachine = playerEntity.GameObject.GetComponent<PlayerStateMachine>();
            
            _graphViewInteraction.ShouldRedo = () => Input.GetKeyDown(KeyCode.X);
            _graphViewInteraction.ShouldUndo = () => Input.GetKeyDown(KeyCode.Z);
            _graphViewInteraction.ShouldTryQuickRemoveAtPointer = () => Input.GetMouseButtonDown(0);
            _graphViewInteraction.ShouldTryQuickRemove = () =>Input.GetKeyDown(KeyCode.P);
            _graphViewInteraction.GetPointerPosition = () =>(Vector2)_uiCamera.ScreenToWorldPoint(Input.mousePosition);
            
            playerStateMachine.ProvideAbilityUseContext = GetAbilityUseContext;
            playerStateMachine.ProvideMovementDirection = GetMovementDirection;
            playerStateMachine.ShouldTryInteract = ShouldTryInteract;
            playerStateMachine.ProvideKeyForAbility = ProvideKeyForAbility;
        }

        private IEnumerable<IAbilityReadOnly> ProvideKeyForAbility(IEnumerable<IAbilityReadOnly> arg)
        {
            List<IAbilityReadOnly> requested = new();
            int index = 0;
            
            foreach (IAbilityReadOnly ability in arg)
            {
                if (Input.GetKey(_abilityKeys[index]))
                {
                    requested.Add(ability);
                }

                index++;
            }

            return requested;
        }

        private bool ShouldTryInteract()
        {
            return Input.GetKeyDown(KeyCode.F);
        }
        
        private Vector2 GetMovementDirection()
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        private AbilityUseContext GetAbilityUseContext()
        {
            Vector3 mousePosition = _gameCamera.ScreenToWorldPoint(Input.mousePosition) * Vector2.one;
            return new AbilityUseContext(mousePosition, _moduleEntity.GameObject.transform.position);
        }
    }
}