using System.Collections.Generic;
using System.Linq;

namespace IM.StateMachines
{
    public class CompositeState : State
    {
        private readonly List<IState> _states;
        
        public CompositeState(IEnumerable<IState> states)
        {
            _states = new List<IState>(states);
        }

        public override void OnEnter() => _states.ForEach(x => x.OnEnter());
        public override void OnExit() => _states.ForEach(x => x.OnEnter());

        public override void Update()
        {
            foreach (IUpdate toUpdate in _states.OfType<IUpdate>()) toUpdate.Update();
        }
        
        public override void FixedUpdate()
        {
            foreach (IFixedUpdate toUpdate in _states.OfType<IFixedUpdate>()) toUpdate.FixedUpdate();
        }
    }
}