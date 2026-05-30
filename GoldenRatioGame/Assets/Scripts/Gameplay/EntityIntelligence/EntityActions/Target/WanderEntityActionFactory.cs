using System;
using IM.Movement;
using IM.Values;
using UnityEngine;

namespace IM.EntityIntelligence
{
    [Serializable]
    public class WanderEntityActionFactory : IEntityActionFactory
    {
        [SerializeField] private CappedValue<float> _walkTime = new(0.5f, 2f);
        [SerializeField] private float _timeBetweenWalking = 2f;
        [SerializeField] private float _biasDistance = 10f;

        public IEntityAction Create(GameObject param1) =>
            new WanderEntityAction(
                param1.transform,
                param1.GetComponent<IVectorMovement>(),
                _walkTime.MinValue,
                _walkTime.MaxValue,
                _timeBetweenWalking,
                _biasDistance
            );
    }
    
    
}