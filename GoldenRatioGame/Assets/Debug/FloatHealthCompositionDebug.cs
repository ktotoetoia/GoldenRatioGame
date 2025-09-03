﻿using UnityEngine;

namespace IM.Health
{
    public class FloatHealthCompositionDebug : MonoBehaviour
    {
        private IFloatHealthComposition _health;

        private void OnDrawGizmos()
        {
            if (_health == null && !TryGetComponent(out _health))
                return;

            var health = _health.Health;
            float ratio = Mathf.InverseLerp(health.MinValue, health.MaxValue, health.Value);

            Vector3 pos = transform.position + Vector3.up * 2.0f;
            Vector3 size = new Vector3(2f, 0.2f, 0f);

            Gizmos.color = Color.gray;
            Gizmos.DrawCube(pos, size);

            Gizmos.color = Color.Lerp(Color.red, Color.green, ratio);
            Vector3 filledSize = new Vector3(size.x * ratio, size.y, size.z);
            Vector3 filledPos = pos - new Vector3((size.x - filledSize.x) / 2f, 0f, 0f);
            Gizmos.DrawCube(filledPos, filledSize);

            if (_health.HealthComponents != null && _health.HealthComponents.Count > 1)
            {
                float totalMax = health.MaxValue;
                float accumulated = 0f;

                foreach (var comp in _health.HealthComponents)
                {
                    accumulated += comp.Health.MaxValue;

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