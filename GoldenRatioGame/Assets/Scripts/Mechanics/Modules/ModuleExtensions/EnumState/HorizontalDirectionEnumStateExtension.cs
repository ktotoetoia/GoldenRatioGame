using System;
using UnityEngine;

namespace IM.Modules
{
    public class HorizontalDirectionEnumStateExtension : MonoBehaviour, IValueStateExtension<PortDirection>
    {
        private PortDirection _portDirection;

        public PortDirection Value
        {
            get => _portDirection;
            set 
            {
                _portDirection = value; 
                
                ValueChanged?.Invoke(value);
            }
        }
        
        public event Action<PortDirection> ValueChanged;
    }
}