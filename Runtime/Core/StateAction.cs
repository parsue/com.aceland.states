using System;

namespace AceLand.States.Core
{
    internal class StateAction
    {
        internal StateAction(Action enter, Action update, Action exit)
        {
            _enter = enter;
            _update = update;
            _exit = exit;
        }

        private Action _enter;
        private Action _update;
        private Action _exit;

        internal bool IsEmpty => _enter is null && _update is null && _exit is null;

        public void Enter()
        {
            _enter?.Invoke();
        }
        
        public void Update()
        {
            _update?.Invoke();
        }

        public void Exit()
        {
            _exit?.Invoke();
        }

        public void RemoveEnter(Action action)
        {
            if (_enter is null) return;
            if (action is null) return;
            if (!_enter.Equals(action)) return;
            _enter = null;
        }
        
        public void RemoveUpdate(Action action)
        {
            if (_update is null) return;
            if (action is null) return;
            if (!_update.Equals(action)) return;
            _update = null;
        }

        public void RemoveExit(Action action)
        {
            if (_exit is null) return;
            if (action is null) return;
            if (!_exit.Equals(action)) return;
            _exit = null;
        }
    }
}