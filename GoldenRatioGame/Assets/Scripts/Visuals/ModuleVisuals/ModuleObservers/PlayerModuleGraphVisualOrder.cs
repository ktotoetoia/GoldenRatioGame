using System.Linq;
using UnityEngine;

namespace IM.Visuals
{
    public class PlayerModuleGraphVisualOrder : MonoBehaviour, IDepthOrderable
    {
        [SerializeField] private PlayerModuleGraphVisualObserver _moduleGraphVisualObserver;
        private int _order;

        public Vector3 ReferencePoint
        {
            get
            {
                return _moduleGraphVisualObserver.ModuleToVisualObjects.Values.OrderBy(x => x.Bounds.min.y).FirstOrDefault()?.Bounds.min ?? default;
            }
        }

        public int Order
        {
            get => _order;
            set 
            { 
                _order = value;
                foreach (IModuleVisualObject visualObject in _moduleGraphVisualObserver.ModuleToVisualObjects.Values)
                {
                    visualObject.Order = value;
                }
            }
        }
    }
}