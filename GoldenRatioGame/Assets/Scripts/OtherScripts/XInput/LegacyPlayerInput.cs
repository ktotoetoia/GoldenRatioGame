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
    [DefaultExecutionOrder(10000)]
    public class LegacyPlayerInput : MonoBehaviour
    {
        [SerializeField] private InitializeSingleEntity  _playerInitialization;
        [SerializeField] private ModuleGraphEditorView _moduleGraphEditor;
        [SerializeField] private DocumentTest _documentTest;
        private PreferredKeyboardBindingsAbilityUser _abilityUser; 
        private IModuleEntity _moduleEntity;
        private IMoveInVector _movement;
        private IStateMachine _stateMachine;

        private void Awake()
        {
            _moduleEntity = _playerInitialization.CreatedEntity as IModuleEntity ?? throw new System.NullReferenceException();
            
            _movement = _moduleEntity.GameObject.GetComponent<IMoveInVector>();
            _stateMachine = new StateMachine(new MovementState(_movement, () => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))));
            _abilityUser = new PreferredKeyboardBindingsAbilityUser(_moduleEntity.AbilityPool);
            _documentTest.SetStorage(_moduleEntity.ModuleEditingContext.Storage);
        }

        private void Update()
        {
            _stateMachine.Update();
            _abilityUser.Update();
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
        
    }
}