using System.Collections.Generic;
using AceLand.Library.Optional;
using AceLand.States.Core;

namespace AceLand.States
{
    public partial class StateMachine : StateMachineBase
    {
        private protected StateMachine(Option<string> id, IState[] states, IAnyState firstState,
            List<StateTransition> anyTransitions, List<StateTransition> transitions) :
            base(id, states, firstState, anyTransitions, transitions) { }
        
        public override IStateMachine Start()
        {
            if (IsActive) return this;

            base.Start();
            return this;
        }

        public override void Stop()
        {
            if (!IsActive) return;
            
            base.Stop();
        }
    }
}
