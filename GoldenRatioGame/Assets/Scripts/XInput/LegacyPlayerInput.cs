using IM.StateMachines;
using IM.Movement;
using UnityEngine;
using IM.ModuleGraphGizmosDebug;
using IM.Modules;

namespace Tests
{
    [DefaultExecutionOrder(1000)]
    public class LegacyPlayerInput : MonoBehaviour
    {
        [SerializeField] private GameObject _playerObject;
        private IMoveInVector _movement;
        private IStateMachine _stateMachine;

        private void Awake()
        {
            GetComponent<GameModuleGizmosDrawer>().Graph = _playerObject.GetComponent<IModuleEntity>().GraphEditor.Graph;
            _movement = _playerObject.GetComponent<IMoveInVector>();
            _stateMachine = new StateMachine(new MovementState(_movement, () => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))));
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }
    }
}