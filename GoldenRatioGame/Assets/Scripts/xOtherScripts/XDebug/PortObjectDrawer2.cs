using UnityEngine;

namespace IM.Visuals
{
    public class PortObjectDrawer2 : MonoBehaviour
    {
        [SerializeField] private bool _draw;
        
        private void OnDrawGizmos()
        {
            if(!_draw) return;
            GizmosPreview(GetComponent<AnimatedModuleVisualObject>());
        }
        
        private void GizmosPreview(AnimatedModuleVisualObject target)
        {
            const float sphereSize = 0.05f;
            const float smallSphereSize = 0.03f;
            const float angleLength = 0.3f;

            foreach (IPortVisualObject portInfo in target.PortsVisualObjects)
            {
                Vector3 position = portInfo.Transform.Position;
                
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(position, sphereSize);
            }
            
            foreach (IPortVisualObject portInfo in target.PortsVisualObjects)
            {
                Vector3 position = portInfo.Transform.Position;
                Quaternion rotation =  Quaternion.Euler(0,0,portInfo.Transform.Rotation.eulerAngles.y);

                Vector3 forward = rotation * Vector3.right;

                Gizmos.color = Color.red;
                
                Gizmos.DrawLine(position, position + forward * angleLength);
                Gizmos.DrawSphere(position + forward* angleLength, smallSphereSize);
            }
        }
    }
}