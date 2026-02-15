using UnityEngine;

namespace IM.Visuals
{
    public class ObjectFlipper : MonoBehaviour
    {
        [SerializeField] private bool _flipX;
        [SerializeField] private bool _flipY;
        [SerializeField] private bool _flipZ;
        
        private bool _flipped;
        
        public bool Flipped 
        {
            get => _flipped;
            set
            {
                if(_flipped == value) return;
                
                _flipped = value;
                transform.localScale = new Vector3(transform.localScale.x * (_flipX ? -1:1), transform.localScale.y* (_flipY ? -1:1), transform.localScale.z* (_flipZ ? -1:1));
                
            }
        }
    }
}