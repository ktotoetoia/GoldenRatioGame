using System.Collections.Generic;
using System.Linq;
using IM.StateMachines;

namespace IM.EntityIntelligence
{
    public class ActionState : State
    {
        private readonly List<IEntityAction> _actions;

        public ActionState(IEnumerable<IEntityAction> actions) => _actions = actions.ToList();

        public override void OnEnter() => _actions.ForEach(a => a.OnEnter());
        public override void Update() => _actions.ForEach(a => a.Update());
        public override void FixedUpdate() => _actions.ForEach(a => a.FixedUpdate());
        public override void OnExit() => _actions.ForEach(a => a.OnExit());
    }
}