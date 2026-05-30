using System;
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
            _stateMachine.UpdateTransition();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_stateMachine?.CurrentState is not ActionState state) return;
           // if (Camera.current != Camera.main) return;
            
            UnityEditor.Handles.Label(
                transform.position + Vector3.up * 2f,
                state.StateName,
                UnityEditor.EditorStyles.boldLabel
            );
        }
#endif
    }
}