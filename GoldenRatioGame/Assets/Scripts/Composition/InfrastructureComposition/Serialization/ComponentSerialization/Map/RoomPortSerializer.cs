using System;
using System.Collections.Generic;
using IM.LifeCycle;
using IM.Map;
using IM.SaveSystem;
using UnityEngine;

namespace IM
{
    public class RoomPortSerializer : ComponentSerializer<RoomPort>
    {
        public override object CaptureState(RoomPort component)
        {
            RoomPortObject state = new();
            
            if (!component.IsConnected || component.Origin is not MonoBehaviour origin ||
                !origin.TryGetComponent(out IIdentifiable originID) ||
                component.Connection is not MonoBehaviour connection ||
                !connection.TryGetComponent(out IIdentifiable connectionID)) return state;

            state.Origin = originID.Id;
            state.Destination = connectionID.Id;
            state.PortSide = (int)component.PortSide;
            
            return state;
        }

        public override void RestoreState(RoomPort component, object state, Func<string, GameObject> resolveDependency)
        {
            RoomPortObject s = (RoomPortObject)state;

            if (s == null)
            {
                Debug.LogWarning("Couldn't load room port");
                return;
            }
            
            IGameObjectRoom origin = resolveDependency(s.Origin).GetComponent<IGameObjectRoom>();
            IRoomPort destination = resolveDependency(s.Destination).GetComponent<IRoomPort>();
            component.PortSide = (PortSide)s.PortSide;
            component.Initialize(origin);
            component.SetDestination(destination);
        }

        private class RoomPortObject
        {
            public string Origin;
            public string Destination;
            public int PortSide;
        }
    }
}