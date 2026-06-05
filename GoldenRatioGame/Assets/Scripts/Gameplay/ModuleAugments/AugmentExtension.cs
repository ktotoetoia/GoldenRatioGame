using System.Collections.Generic;
using UnityEngine;

namespace IM.Augments
{
    public class AugmentExtension : MonoBehaviour, IAugmentExtension
    {
        [SerializeField] private List<AugmentInfo> _augments = new();

        public IEnumerable<AugmentInfo> Augments => _augments;
    }
}