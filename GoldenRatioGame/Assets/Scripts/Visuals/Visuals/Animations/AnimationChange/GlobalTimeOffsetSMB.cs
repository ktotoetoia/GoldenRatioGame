using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Visuals
{
    public class GlobalTimeOffsetSMB : StateMachineBehaviour
    {
        public override void OnStateEnter(
            Animator animator,
            AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            float offset = ((Time.time) % stateInfo.length) / stateInfo.length;
            animator.Update(offset * stateInfo.length);
        }
    }
}