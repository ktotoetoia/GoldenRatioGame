using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Health
{
    public class FloatHealthValuesGroupDebug : MonoBehaviour
    {
        [SerializeField] private bool _isOn = true;
        private IReadOnlyCollection<GameObject> _gameObjectsCollection;

        private void Awake()
        {
            _gameObjectsCollection = GetComponent<IReadOnlyCollection<GameObject>>();
        }

        private void OnDrawGizmos()
        {
            if(!_isOn || !Application.isPlaying) return;
            
            foreach (GameObject target in _gameObjectsCollection.Where(x => x.activeInHierarchy && x.TryGetComponent(out IFloatHealthValuesGroup h)))
            {
                DrawFor(target);
            }
        }

        private void DrawFor(GameObject target)
        {
            IFloatHealthValuesGroup healthValuesGroup = target.GetComponent<IFloatHealthValuesGroup>();
            var health = healthValuesGroup.Health;
            float ratio = Mathf.InverseLerp(health.MinValue, health.MaxValue, health.Value);

            Vector3 pos = target.transform.position + Vector3.up * 2.0f;
            Vector3 size = new Vector3(2f, 0.2f, 0f);
            
            Gizmos.color = Color.gray;
            Gizmos.DrawCube(pos, size);

            Gizmos.color = Color.Lerp(Color.red, Color.green, ratio);
            Vector3 filledSize = new Vector3(size.x * ratio, size.y, size.z);
            Vector3 filledPos = pos - new Vector3((size.x - filledSize.x) / 2f, 0f, 0f);
            Gizmos.DrawCube(filledPos, filledSize);
            
            if (healthValuesGroup.Values is { Count: > 1 })
            {
                float totalMax = health.MaxValue;
                float accumulated = 0f;

                foreach (var comp in healthValuesGroup.Values)
                {
                    accumulated += comp.MaxValue;

                    if (accumulated >= totalMax)
                        break;

                    float t = accumulated / totalMax;
                    float xOffset = (t - 0.5f) * size.x;

                    Vector3 lineStart = pos + new Vector3(xOffset, size.y * 0.5f, 0f);
                    Vector3 lineEnd   = pos - new Vector3(0f, size.y * 0.5f, 0f) + new Vector3(xOffset, 0f, 0f);

                    Gizmos.color = Color.black;
                    Gizmos.DrawLine(lineStart, lineEnd);
                }
            }
        }
    }
}