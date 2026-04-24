using IM.Modules;
using UnityEngine;

namespace IM.UI
{
    public class ContextViewer : MonoBehaviour
    {
        public virtual void SetContext(IModuleEditingContext context) { }
        public virtual void ClearContext() { }
    }
}