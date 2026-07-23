using System.Collections.Generic;
using System.Linq;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    [CreateAssetMenu(menuName = "Map/Prop Room Decorator")]
    public class PropRoomDecorator : RoomDecorator
    {
        [SerializeField] private List<PropSet> _propSets;
        [SerializeField] private float _portClearance = 1f;

        public override IEnumerable<GameObject> Decorate(IGameObjectRoom room, IRoomShape shape, IGameObjectFactory factory)
        {
            RoomDecorationContext decorationContext = new RoomDecorationContext(shape, 1,room.RoomPorts.Select(x => (Vector2)x.EnterPosition).ToList(),_portClearance);
            List<GameObject> props = new List<GameObject>();
            
            foreach (PropSet propSet in _propSets)
            {
                int count = Random.Range(propSet.MinCount, propSet.MaxCount+1);
                
                if(!propSet.PropInfo || count <= 0 || propSet.PropPlacementStrategy == null) continue;
                
                int i = 0;
                
                foreach (PropPlacementInfo propPlacement in propSet.PropPlacementStrategy.GetPlacements(decorationContext, propSet.PropInfo.CellSize))
                {
                    if (i >= count) break;

                    GameObject prop = factory.Create(propSet.PropInfo.Prefab, false);

                    room.Add(prop);
                    props.Add(prop);
                    decorationContext.Add(propPlacement,propSet.PropInfo.ClearanceRadius);
                    prop.transform.localPosition = propPlacement.Center - new Vector2(shape.Metrics.TotalW/2f, shape.Metrics.TotalH/2f) + Vector2.one;
                    prop.transform.localPosition += new Vector3(shape.Metrics.TotalW % 2 == 0 ? 0.5f : 0, shape.Metrics.TotalH % 2 == 0 ? -0.5f: 0);
                    
                    i++;
                }
            }

            return props;
        }
    }
}