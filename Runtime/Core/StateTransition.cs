using System;

namespace AceLand.States.Core
{
    internal class StateTransition
    {
        internal StateTransition(IAnyState from, IAnyState to, Func<bool> argument, bool preventToSelf)
        {
            From = from;
            To = to;
            Argument = argument;
            PreventToSelf = preventToSelf;
        }

        public IAnyState From { get; }
        public IAnyState To { get; }
        private Func<bool> Argument { get; }
        private bool PreventToSelf { get; }

        internal bool IsNextState(IAnyState currentState, out IAnyState nextAnyState)
        {
            nextAnyState = null;
            if (From is not AnyState && currentState != From) return false;
            if (PreventToSelf && currentState == To) return false;
            if (!Argument.Invoke()) return false;
            
            nextAnyState = To;
            return true;
        }

        internal bool TryGetState(string name, out IAnyState anyState)
        {
            if (From.Name == name)
            {
                anyState = From;
                return true;
            }
            if (To.Name == name)
            {
                anyState = To;
                return true;
            }

            anyState = null;
            return false;
        }
    }
}