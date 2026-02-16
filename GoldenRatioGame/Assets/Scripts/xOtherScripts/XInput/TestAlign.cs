using System.Collections.Generic;
using UnityEngine;

namespace Tests
{
    public class TestAlign : MonoBehaviour
    {
        [SerializeField] private List<Transform> _from;
        [SerializeField] private List<Transform> _to;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                for (int i = 0; i < _from.Count; i++)
                {
                    AlignPorts(_from[i], _to[i]);
                }
            }
        }
        
        public void AlignPorts(Transform portToMove, Transform anchorPort)
        {
            Transform owner = portToMove.parent;
            
            owner.rotation = GetRotation(portToMove, anchorPort);
            owner.position = GetPosition(portToMove, anchorPort, owner.rotation);
        }
        
        private Quaternion GetRotation(Transform portToMove, Transform anchorPort)
        {
            Transform owner = portToMove.parent;

            Quaternion portLocal =
                Quaternion.Inverse(owner.rotation) *
                portToMove.rotation;
            
            Quaternion result = anchorPort.rotation * Quaternion.Inverse(portLocal);

            if (Mathf.Sign(portToMove.lossyScale.x) == Mathf.Sign(anchorPort.lossyScale.x))
            {
                result *= Quaternion.Euler(0,0,180);
            }

            return result;
        }
        
        private Vector3 GetPosition(Transform portToMove, Transform anchorPort, Quaternion ownerRot)
        {
            Vector3 portLocalPos = portToMove.localPosition;
            Vector3 ownerScale = portToMove.parent.lossyScale;
            Vector3 offset = ownerRot * Vector3.Scale(ownerScale, portLocalPos);

            return anchorPort.position - offset;
        }
    }
}