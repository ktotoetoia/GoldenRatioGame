using IM.StateMachines;
using TD.Movement;
using UnityEngine;

namespace Tests
{
    public class LegacyPlayerInput : MonoBehaviour
    {
        private IMoveInVector _movement;
        private IStateMachine _stateMachine;

        private void Awake()
        {
            _movement = GetComponent<IMoveInVector>();
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