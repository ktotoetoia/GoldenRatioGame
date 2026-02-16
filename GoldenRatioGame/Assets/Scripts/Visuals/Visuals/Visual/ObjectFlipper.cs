using UnityEngine;

namespace IM.Visuals
{
    public class ObjectFlipper : MonoBehaviour
    {
        [SerializeField] private bool _flipX = true;
        [SerializeField] private bool _flipY;
        [SerializeField] private bool _flipZ;

        private bool _flipped;

        public bool Flipped
        {
            get => _flipped;
            set
            {
                _flipped = value;

                var scale = transform.localScale;

                if (_flipX)
                    scale.x = Mathf.Abs(scale.x) * (_flipped ? -1f : 1f);

                if (_flipY)
                    scale.y = Mathf.Abs(scale.y) * (_flipped ? -1f : 1f);

                if (_flipZ)
                    scale.z = Mathf.Abs(scale.z) * (_flipped ? -1f : 1f);

                transform.localScale = scale;
            }
        }
    }
}