using UnityEngine;

namespace IM.EntityIntelligence
{
    public class DebugEntityAction : EntityAction
    {
        private readonly bool _printOnEnter;
        private readonly bool _printOnExit;
        private readonly bool _printOnUpdate;
        private readonly bool _printOnFixedUpdate;

        public string Text { get; set; } = "Text";
        
        public DebugEntityAction(bool printOnEnter = true, bool printOnExit = false,bool printOnUpdate = false,bool printOnFixedUpdate= false)
        {
            _printOnEnter = printOnEnter;
            _printOnExit = printOnExit;
            _printOnUpdate = printOnUpdate;
            _printOnFixedUpdate = printOnFixedUpdate;
        }

        public override void OnEnter()
        {
            if(_printOnEnter) Debug.Log("On Enter: " + Text);
        }

        public override void OnExit()
        {
            if(_printOnExit) Debug.Log("On Exit: " + Text);
        }

        public override void Update()
        {
            if(_printOnUpdate) Debug.Log("On Update: " + Text);
        }

        public override void FixedUpdate()
        {
            if (_printOnFixedUpdate) Debug.Log("On FixedUpdate: " + Text);
        }
    }
}