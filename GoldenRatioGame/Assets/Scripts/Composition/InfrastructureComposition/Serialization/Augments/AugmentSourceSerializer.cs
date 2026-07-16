using System;
using System.Collections.Generic;
using IM.Augments;
using IM.LifeCycle;
using IM.SaveSystem;
using UnityEngine;

namespace IM.Modules
{
    public class AugmentSourceSerializer : ComponentSerializer<AugmentSource>
    {
        public override object CaptureState(AugmentSource component)
        {
            List<EntityProgression> progressions = new();

            foreach (IAugmentProgress augmentProgress in component.Progresses)
            {
                if (augmentProgress.Entity.GameObject.TryGetComponent(out IIdentifiable identifiable))
                {
                    progressions.Add(new EntityProgression(identifiable.Id,augmentProgress.Progress));
                }
            }
            
            return progressions;
        }

        public override void RestoreState(AugmentSource component, object state, Func<string, GameObject> resolveDependency)
        {
            foreach (EntityProgression entityProgression in (List<EntityProgression>)state)
            {
                if (resolveDependency(entityProgression.EntityID).TryGetComponent(out IEntity entity))
                {
                    component.AddProgress(entity,entityProgression.Progress);
                }
            }
        }
        
        [Serializable]
        private class EntityProgression
        {
            public string EntityID;
            public AugmentProgressInfo Progress;

            public EntityProgression(string entityID, AugmentProgressInfo progress)
            {
                EntityID = entityID;
                Progress = progress;
            }
        }
    }
}