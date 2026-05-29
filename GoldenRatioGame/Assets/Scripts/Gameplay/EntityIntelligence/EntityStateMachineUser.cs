using IM.StateMachines;
using UnityEngine;

namespace IM.EntityIntelligence
{
    public class EntityStateMachineUser : MonoBehaviour
    {
        [SerializeField] private StateMachineFactory _stateMachineFactory;
        private IStateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine = _stateMachineFactory.Create(gameObject);
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