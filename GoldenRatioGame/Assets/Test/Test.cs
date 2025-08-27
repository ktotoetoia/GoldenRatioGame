using IM.GoldenRatio;
using UnityEngine;

namespace Tests
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private float _arcStep;
        [SerializeField] private float _maxS;
        [SerializeField] private float _rotation;
        
        private void OnDrawGizmos()
        {
            PointsBasedGoldenRatio ratio = new PointsBasedGoldenRatio(_arcStep, _maxS, _rotation);

            foreach (Vector2 point in ratio.Points)
            {
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }
}