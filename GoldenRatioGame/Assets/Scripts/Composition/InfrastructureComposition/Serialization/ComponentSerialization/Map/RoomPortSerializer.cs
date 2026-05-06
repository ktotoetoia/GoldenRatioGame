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
            List<string> state = new();
            if (!component.IsConnected || component.Origin is not MonoBehaviour origin ||
                !origin.TryGetComponent(out IIdentifiable originID) ||
                component.Connection is not MonoBehaviour connection ||
                !connection.TryGetComponent(out IIdentifiable connectionID)) return state;
            
            state.Add(originID.Id);
            state.Add(connectionID.Id);
            
            return state;
        }

        public override void RestoreState(RoomPort component, object state, Func<string, GameObject> resolveDependency)
        {
            List<string> s = (List<string>)state;

            if (s.Count == 0)
            {
                Debug.LogWarning("Couldn't load room port");
                return;
            }
            
            IGameObjectRoom origin = resolveDependency(s[0]).GetComponent<IGameObjectRoom>();
            IRoomPort destination = resolveDependency(s[1]).GetComponent<IRoomPort>();
            
            component.Initialize(origin);
            component.SetDestination(destination);
        }
    }
}