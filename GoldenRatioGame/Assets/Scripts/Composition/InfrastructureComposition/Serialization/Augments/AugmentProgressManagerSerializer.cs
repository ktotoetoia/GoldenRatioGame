using System;
using System.Collections.Generic;
using IM.Augments;
using IM.LifeCycle;
using IM.SaveSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace IM.Modules
{
    public class AugmentProgressManagerSerializer : ComponentSerializer<AugmentProgressManager>
    {
        public override object CaptureState(AugmentProgressManager component)
        {
            Dictionary<string, object> state = new Dictionary<string,object>();

            foreach (var a  in component.AppliedFactories)
            {
                if (a.Value is not IHaveID id) continue;

                object saved = a.Value.Save(a.Key);
                
                state[id.Id] = saved; 
            }

            return state;
        }

        public override void RestoreState(AugmentProgressManager component, object state, Func<string, GameObject> resolveDependency)
        {
            if (state is not Dictionary<string, object> savedState)
            {
                Debug.LogWarning("Failed to restore AugmentProgressManager: state is invalid or null.");
                return;
            }

            Dictionary<IAugment,IAugmentFactory> restoredFactories = new ();

            foreach (var (factoryId, augmentSave) in savedState)
            {
                IAugmentFactory factory = ResolveFactoryById(factoryId);
                
                if (factory != null)
                {
                    IAugment restoredAugment = factory.Restore(augmentSave,new AugmentContext(component.gameObject));
                    restoredFactories[restoredAugment] = factory;
                }
                else
                {
                    Debug.LogWarning($"Could not find IAugmentFactory with ID: {factoryId}");
                }
            }

            foreach ((IAugment augment, IAugmentFactory factory) in restoredFactories)
            {
                component.AddAppliedFactory(augment,factory);
            }
        }

        private IAugmentFactory ResolveFactoryById(string id)
        {
            var handle = Addressables.LoadAssetAsync<IAugmentFactory>(id);
            return handle.WaitForCompletion(); 
         }
    }
}