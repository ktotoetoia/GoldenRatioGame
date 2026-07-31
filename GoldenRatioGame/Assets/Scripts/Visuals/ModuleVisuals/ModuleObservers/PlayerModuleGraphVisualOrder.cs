using System.Linq;
using UnityEngine;

namespace IM.Visuals
{
    public class PlayerModuleGraphVisualOrder : MonoBehaviour, IDepthOrderable
    {
        [SerializeField] private EntityModuleGraphVisualObserver _moduleGraphVisualObserver;
        private int _order;
        
        [field:SerializeField]  public float Elevation { get; private set; }
        [field:SerializeField]  public float Height { get;  private set;}
        [field:SerializeField]  public float HalfWidth { get;  private set;}

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