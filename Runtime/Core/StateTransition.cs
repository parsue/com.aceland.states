using System;
using UnityEngine;

namespace AceLand.States.Core
{
    internal class StateTransition
    {
        internal StateTransition(IAnyState from, IAnyState to, Func<bool> argument, bool preventToSelf)
        {
            _from = from;
            _to = to;
            _argument = argument;
            _preventToSelf = preventToSelf;
        }

        private readonly IAnyState _from;
        private readonly IAnyState _to;
        private readonly Func<bool> _argument;
        private readonly bool _preventToSelf;

        internal bool IsNextState(IAnyState currentState, out IAnyState nextAnyState)
        {
            nextAnyState = null;
            if (_from != StatesUtils.AnyState && !currentState.CompareTo(_from)) return false;
            if (_preventToSelf && currentState.CompareTo(_to)) return false;
            if (!_argument.Invoke()) return false;
            
            nextAnyState = _to;
            return true;
        }

        internal bool TryGetState(string name, out IAnyState anyState)
        {
            if (_from.Name == name)
            {
                anyState = _from;
                return true;
            }
            if (_to.Name == name)
            {
                anyState = _to;
                return true;
            }

            anyState = null;
            return false;
        }
    }
}