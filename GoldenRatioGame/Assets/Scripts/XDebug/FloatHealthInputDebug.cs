using UnityEditor;
using UnityEngine;

namespace IM.Health
{
    public class FloatHealthInputDebug : MonoBehaviour
    {
        [SerializeField] private GameObject _target;
        [SerializeField] private float _value;
        private IFloatHealth _health;
        private HealthChangeResult _healthChangeResult;
        private bool _hasResult;

        private void Awake()
        {
            _target?.TryGetComponent(out _health);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                _healthChangeResult = _health.TakeDamage(_value); 
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                _healthChangeResult = _health.RestoreHealth(_value); 
            }
        }

        private void OnDrawGizmos()
        {
            Vector3 pos = _target.transform.position + Vector3.up * 3.5f;

            string text =
                $"PreMitigation: {_healthChangeResult.PreMitigation}\n" +
                $"PostMitigationValue: {_healthChangeResult.PostMitigationUnclamped}\n" +
                $"Applied: {_healthChangeResult.Applied}\n" +
                $"Mitigated: {_healthChangeResult.Mitigated}\n" +
                $"Overflow: {_healthChangeResult.Overflow}";

            Handles.Label(pos, text);
        }
    }
}