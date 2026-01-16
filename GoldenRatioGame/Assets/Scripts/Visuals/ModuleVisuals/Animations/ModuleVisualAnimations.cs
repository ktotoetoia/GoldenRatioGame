using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleVisualAnimations : MonoBehaviour
    {
        private readonly List<IAnimationChange>  _animationChange = new();
        private IModuleVisual _moduleVisual;
        
        private void Awake()
        {
            _moduleVisual = GetComponent<IModuleVisual>() ?? throw new NullReferenceException(nameof(IModuleVisual));
            GetComponents(_animationChange);
        }

        private void Update()
        {
            if(_moduleVisual is not { ReferenceModuleVisualObject: IModuleVisualObjectAnimated animated }) return;

            foreach (IAnimationChange animationChange in _animationChange)
            {
                animationChange.ApplyToAnimator(animated.Animator);
            }
        }
    }
}