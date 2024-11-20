using System.Collections.Generic;

namespace AceLand.States.Core
{
    internal static class StateTransitionExtension
    {
        public static bool IsStateTransition(this IStateMachine machine,
            IEnumerable<StateTransition> anyTransitions, IEnumerable<StateTransition> transitions,
            out IAnyState nextState, out bool isAny)
        {
            var currentState = machine.CurrentState;
            foreach (var transition in anyTransitions)
            {
                if (!transition.IsNextState(currentState, out nextState)) continue;
                isAny = true;
                return true;
            }

            foreach (var transition in transitions)
            {
                if (!transition.IsNextState(currentState, out nextState)) continue;
                isAny = false;
                return true;
            }

            nextState = null;
            isAny = false;
            return false;
        }
    }
}