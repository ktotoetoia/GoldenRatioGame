using System.Collections.Generic;
using IM.Abilities;
using IM.Entities;
using IM.LifeCycle;
using IM.Modules;
using IM.UI;
using IM.Values;
using IM.Visuals;
using UnityEngine;

namespace IM.Inputs
{
    public class LegacyPlayerInput : MonoBehaviour, IRequirePlayerEntity
    {
        [SerializeField] private EntityContextEditingViewer entityContextEditingViewer;
        [SerializeField] private GraphViewInteraction _graphViewInteraction;
        [SerializeField] private EditButtonsContextViewer _contextViewer;
        [SerializeField] private Camera _gameCamera;
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private PauseManager _pauseManager;
        [SerializeField] private List<KeyCode> _abilityKeys =  new ()
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Q,
            KeyCode.E
        };
        
        private IModuleEntity _moduleEntity;
        private IAbilityAnchorPositionProvider _abilityAnchorPositionProvider;
        private IModuleEditingContext _trySave;
        private bool _shouldSave = true;
        private bool _shouldExit;
        
        private void Update()
        {
            _contextViewer.SetSaveAndExitButtonEnabled(_moduleEntity.ModuleEditingContextEditor.CanApplyChanges);
            if(Input.GetKeyDown(KeyCode.Escape)) _pauseManager.SetPaused(!_pauseManager.Paused);
        }

        public void SetPlayerEntity(IEntity playerEntity)
        {
            _moduleEntity = playerEntity as IModuleEntity;
            _abilityAnchorPositionProvider = playerEntity.GameObject.GetComponent<IAbilityAnchorPositionProvider>();
            PlayerStateMachine playerStateMachine = playerEntity.GameObject.GetComponent<PlayerStateMachine>();
            

            _contextViewer.SaveAndExitButtonClicked += () =>
            {
                _shouldSave = true;
                _shouldExit = true;
            };

            _contextViewer.ExitButtonClicked += () =>
            {
                _shouldSave = false;
                _shouldExit = true;
            };
            
            _graphViewInteraction.ShouldRedo = () => Input.GetKeyDown(KeyCode.X);
            _graphViewInteraction.ShouldUndo = () => Input.GetKeyDown(KeyCode.Z);
            _graphViewInteraction.ShouldTryQuickRemoveAtPointer = () => Input.GetMouseButtonDown(0);
            _graphViewInteraction.ShouldTryQuickRemove = () =>Input.GetKeyDown(KeyCode.P);
            _graphViewInteraction.GetPointerPosition = () =>(Vector2)_uiCamera.ScreenToWorldPoint(Input.mousePosition);

            playerStateMachine.ShouldTryStartEditing = () => Input.GetKeyDown(KeyCode.I) || _shouldExit;
            playerStateMachine.ShouldTryStopEditing = () => Input.GetKeyDown(KeyCode.I) || _shouldExit;
            playerStateMachine.EditStarted += x =>entityContextEditingViewer.SetModuleEditingContext(_moduleEntity,x);
            playerStateMachine.EditEnded += () =>
            {
                _shouldSave = true;
                _shouldExit = false;
                entityContextEditingViewer.ClearModuleEditingContext();
            };
            playerStateMachine.ProvideMovementDirection = GetMovementDirection;
            playerStateMachine.ShouldTryInteract = ShouldTryInteract;
            playerStateMachine.ResolveRequestedAbilities = ProvideKeyForAbility;
            playerStateMachine.ShouldTrySaveChanges = x => _shouldSave;
        }

        private IEnumerable<KeyValuePair<IAbilityReadOnly,UseContext>> ProvideKeyForAbility(IEnumerable<IAbilityReadOnly> arg)
        {
            Dictionary<IAbilityReadOnly, UseContext> requested = new();
            int index = 0;
            
            foreach (IAbilityReadOnly ability in arg)
            {
                if (Input.GetKey(_abilityKeys[index]))
                {
                    requested.Add(ability,GetAbilityUseContext(ability));
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

        private UseContext GetAbilityUseContext(IAbilityReadOnly ability)
        {
            Vector3 mousePosition = _gameCamera.ScreenToWorldPoint(Input.mousePosition) * Vector2.one;
            return new UseContext(mousePosition, _moduleEntity.GameObject.transform.position,
                _abilityAnchorPositionProvider?.GetAnchorPosition(ability) ?? _moduleEntity.GameObject.transform.position);
        }
    }
}