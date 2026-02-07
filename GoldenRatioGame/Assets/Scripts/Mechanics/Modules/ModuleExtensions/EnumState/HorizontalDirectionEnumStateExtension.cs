using System;
using UnityEngine;

namespace IM.Modules
{
    public class HorizontalDirectionEnumStateExtension : MonoBehaviour, IEnumStateExtension<HorizontalDirection>
    {
        private HorizontalDirection _horizontalDirection;

        public HorizontalDirection Value
        {
            get=> _horizontalDirection;
            set 
            {
                _horizontalDirection = value; 
                
                ValueChanged?.Invoke(value);
            }
        }
        
        public event Action<HorizontalDirection> ValueChanged;
    }
}