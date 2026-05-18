using System;
using UnityEngine;

namespace IM.Map
{
    public class TransitionPoint :  MonoBehaviour, ITransitionPoint
    {
        public bool IsOpen { get; set; }
        public event Action Interacted;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsOpen && other.gameObject.TryGetComponent(out IRoomWalker roomWalker))
            {
                Interacted?.Invoke();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (IsOpen && other.gameObject.TryGetComponent(out IRoomWalker roomWalker))
            {
                Interacted?.Invoke();
            }
        }
    }
}