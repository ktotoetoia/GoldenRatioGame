using System.Linq;
using IM.Abilities;
using IM.Visuals;
using UnityEngine;

namespace IM
{
    public class ViewAnchorPositionProvider : MonoBehaviour, IAbilityAnchorPositionProvider
    {
        [SerializeField] private GameObject _moduleVisualMapSource;
        private IModuleVisualMap _moduleVisualMap;

        private void Awake()
        {
            _moduleVisualMap = _moduleVisualMapSource.GetComponent<IModuleVisualMap>();
        }

        public Vector3 GetAnchorPosition(IAbilityReadOnly ability)
        {
            return GetPosition(_moduleVisualMap.ModuleToVisualObjects.FirstOrDefault(x =>
                x.Key.Value.Extensions.TryGet(out IAbilityContainer e) && e.Ability.Equals(ability)).Value);
        }

        private static Vector3 GetPosition(IModuleVisualObject moduleVisualObject)
        {
            
            return moduleVisualObject.Transform.Position;
        }
    }
}