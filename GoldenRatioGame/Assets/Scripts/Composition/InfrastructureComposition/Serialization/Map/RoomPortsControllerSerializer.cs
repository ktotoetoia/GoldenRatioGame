using System;
using IM.Map;
using IM.SaveSystem;
using UnityEngine;

namespace IM
{
    public class RoomPortsControllerSerializer : ComponentSerializer<RoomPortsController>
    {
        public override object CaptureState(RoomPortsController component)
        {
            return component.Cleared;
        }

        public override void RestoreState(RoomPortsController component, object state, Func<string, GameObject> resolveDependency)
        {
            component.Cleared = (bool)state;
        }
    }
}