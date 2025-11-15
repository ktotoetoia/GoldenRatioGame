using UnityEngine;

namespace IM.Health
{
    public class FloatHealthDebug : MonoBehaviour
    {
        [SerializeField] private GameObject _target;
        [SerializeField] private bool _isOn = true;
        private IFloatHealth _health;
        
        private void OnDrawGizmos()
        {
            if (_target == null || _health == null && !_target.TryGetComponent(out _health) || !_isOn)
                return;

            float ratio = Mathf.InverseLerp(_health.Health.MinValue, _health.Health.MaxValue, _health.Health.Value);
            
            Vector3 pos = _target.transform.position + Vector3.up * 1.5f;
            Vector3 size = new Vector3(2f, 0.2f, 0f);

            Gizmos.color = Color.gray;
            Gizmos.DrawCube(pos, size);
            Gizmos.color = Color.Lerp(Color.red, Color.green, ratio);
            
            Vector3 filledSize = new Vector3(size.x * ratio, size.y, size.z);
            Vector3 filledPos = pos - new Vector3((size.x - filledSize.x) / 2f, 0f, 0f);

            Gizmos.DrawCube(filledPos, filledSize);
        }
    }
}