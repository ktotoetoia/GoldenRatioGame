using UnityEditor;
using UnityEngine;

namespace IM.Health
{
    public class FloatHealthInputDebug : MonoBehaviour
    {
        [SerializeField] private float _value;
        private IFloatHealth _health;
        private HealthChangeResult _healthChangeResult;
        private bool _hasResult;

        private void Awake()
        {
            _health = GetComponent<IFloatHealth>();
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
            Vector3 pos = transform.position + Vector3.up * 3f;

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