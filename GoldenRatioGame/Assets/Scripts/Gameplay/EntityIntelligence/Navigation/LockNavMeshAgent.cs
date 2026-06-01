using UnityEngine;
using UnityEngine.AI;

namespace IM.EntityIntelligence
{
    public class LockNavMeshAgent : MonoBehaviour
    {
        private void Awake()
        {
            var agent = GetComponent<NavMeshAgent>();
    
            agent.updateRotation = false; 
            agent.updateUpAxis = false; 
        }
    }
}