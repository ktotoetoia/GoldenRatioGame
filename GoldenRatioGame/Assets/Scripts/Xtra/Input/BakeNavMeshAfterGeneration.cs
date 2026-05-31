using System.Runtime.Remoting.Contexts;
using NavMeshPlus.Components;
using UnityEngine;

namespace IM.Inputs
{
    public class BakeNavMeshAfterGeneration : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface surface;

        [ContextMenu("Rebuild")]
        public void Rebuild()
        {
            surface.BuildNavMesh();
        }
    }
}