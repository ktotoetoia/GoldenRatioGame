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
            var state = new AugmentProgressManagerState();

            foreach (KeyValuePair<IAugmentExtension, AugmentProgressInfo> kvp in component.AugmentProgress)
            {
                if(kvp.Key is not MonoBehaviour mb || !mb || !mb.TryGetComponent(out IHaveID id)) continue;
                
                state.Progress[id.Id] = kvp.Value;
            }

            foreach (IAugmentFactory factory in component.AppliedFactories)
            {
                if (factory is not IHaveID id) continue;
                
                state.AppliedFactoryIds.Add(id.Id); 
            }

            return state;
        }

        public override void RestoreState(AugmentProgressManager component, object state, Func<string, GameObject> resolveDependency)
        {
            if (state is not AugmentProgressManagerState savedState)
            {
                Debug.LogWarning("Failed to restore AugmentProgressManager: state is invalid or null.");
                return;
            }

            var restoredProgress = new Dictionary<IAugmentExtension, AugmentProgressInfo>();
            var restoredFactories = new List<IAugmentFactory>();

            foreach (KeyValuePair<string, AugmentProgressInfo> kvp in savedState.Progress)
            {
                IAugmentExtension extension = ResolveExtensionById(kvp.Key,resolveDependency);

                if (extension != null)
                {
                    restoredProgress[extension] = kvp.Value;
                }
                else
                {
                    Debug.LogWarning($"Could not find IAugmentExtension with ID: {kvp.Key}");
                }
            }

            foreach (string factoryId in savedState.AppliedFactoryIds)
            {
                IAugmentFactory factory = ResolveFactoryById(factoryId);
                if (factory != null)
                {
                    restoredFactories.Add(factory);
                }
                else
                {
                    Debug.LogWarning($"Could not find IAugmentFactory with ID: {factoryId}");
                }
            }

            component.AddToProgress(restoredProgress, restoredFactories);
        }
        
        private IAugmentExtension ResolveExtensionById(string id,Func<string, GameObject> resolveDependency)
        {;
            return resolveDependency(id)?.GetComponent<IAugmentExtension>();
        }

        private IAugmentFactory ResolveFactoryById(string id)
        {
            var handle = Addressables.LoadAssetAsync<IAugmentFactory>(id);
            return handle.WaitForCompletion(); 
         }
    }
    
    [Serializable]
    public class AugmentProgressManagerState
    {
        public Dictionary<string, AugmentProgressInfo> Progress = new();
        public List<string> AppliedFactoryIds = new();
    }
}