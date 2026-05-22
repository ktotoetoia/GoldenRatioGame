using System;
using IM.Map;
using IM.SaveSystem;
using UnityEngine;

namespace IM
{
    public class RectangleRoomFormSerializer : ComponentSerializer<RectangleRoomForm>
    {
        public override object CaptureState(RectangleRoomForm component)
        {
            return component.Rect;
        }

        public override void RestoreState(RectangleRoomForm component, object state, Func<string, GameObject> resolveDependency)
        {
            component.SetRect((Rect)state);
        }
    }
}