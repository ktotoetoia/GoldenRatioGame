using UnityEngine;

namespace IM.Modules
{
    public class ModuleRenderer : MonoBehaviour
    {
        private bool _visibility;

        public bool Visibility
        {
            get => _visibility;
            set
            { 
                _visibility = value;

                if (value)
                {
                    
                }
            }
        }
    }
}