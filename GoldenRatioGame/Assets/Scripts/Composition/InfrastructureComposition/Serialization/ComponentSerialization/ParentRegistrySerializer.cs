using System;
using System.Collections.Generic;
using IM.LifeCycle;
using IM.SaveSystem;
using UnityEngine;

namespace IM.Modules
{
    public class ParentRegistrySerializer : ComponentSerializer<ParentRegistry>
    {
        public override object CaptureState(ParentRegistry component)
        {
            List<string> ids = new List<string>();

            foreach (IParentRestorable parentRestorable in component.TrackedObjects)
            {
                if (parentRestorable is MonoBehaviour mb && mb.TryGetComponent(out IIdentifiable id))
                {
                    ids.Add(id.Id);
                }
            }
            
            return ids;
        }

        public override void RestoreState(ParentRegistry component, object state, Func<string, GameObject> resolveDependency)
        {
            List<string> ids = (List<string>)state;

            foreach (string id in ids)
            {
                component.Register(resolveDependency(id).GetComponent<IParentRestorable>());
            }
        }
    }
}