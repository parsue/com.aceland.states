using System;
using System.Collections.Generic;

namespace AceLand.States.Core
{
    internal sealed class StateUpdater
    {
        internal StateUpdater() { }

        internal StateUpdater(List<StateAction> actions)
            => _actions.AddRange(actions);
        
        private readonly List<StateAction> _actions = new();
        private StateStep _step;
        private bool _isEntered;
        
        public void Enter()
        {
            _step = StateStep.Enter;
            foreach (var action in _actions)
                action?.Enter();
            _isEntered = true;
        }
        
        public void Update()
        {
            _step = StateStep.Update;
            foreach (var action in _actions)
                action?.Update();
        }

        public void Exit()
        {
            _step = StateStep.Exit;
            foreach (var action in _actions)
                action?.Exit();
            _isEntered = false;
        }

        public void InjectActions(Action enter = null, Action update = null, Action exit = null)
        {
            _actions.Add(StatesHelper.CreateStateUpdater(enter, update, exit));
            if (_isEntered && _step is not StateStep.Exit)
                enter?.Invoke();
        }

        public void RemoveActions(Action enter = null, Action update = null, Action exit = null)
        {
            var emptyActions = new List<int>();
            for (var i = _actions.Count - 1; i >= 0; i--)
            {
                var action = _actions[i];
                action.RemoveEnter(enter);
                action.RemoveUpdate(update);
                action.RemoveExit(exit);
                if (action.IsEmpty) emptyActions.Add(i);
            }
            foreach (var index in emptyActions)
                _actions.RemoveAt(index);
        }
    }
}
