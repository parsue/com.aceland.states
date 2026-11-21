using System;
#if ENABLE_PROFILER
using Unity.Profiling;
#endif

namespace AceLand.States.Core
{
    internal class StateAction
    {
        internal StateAction(Action enter, Action update, Action exit)
        {
            _enter = enter;
            _update = update;
            _exit = exit;
#if ENABLE_PROFILER
            // Name each injected action's marker after the source delegate (DeclaringType.Method),
            // so the Profiler links the measurement back to the user function. '.' separators only.
            _enterMarker = MakeMarker("Enter", enter);
            _updateMarker = MakeMarker("Update", update);
            _exitMarker = MakeMarker("Exit", exit);
#endif
        }

        private Action _enter;
        private Action _update;
        private Action _exit;

#if ENABLE_PROFILER
        private ProfilerMarker _enterMarker;
        private ProfilerMarker _updateMarker;
        private ProfilerMarker _exitMarker;

        private static ProfilerMarker MakeMarker(string step, Action action)
        {
            var method = action?.Method;
            var name = method is null
                ? "AceLand.States.Action.<none>." + step
                : "AceLand.States.Action." +
                  (method.DeclaringType?.Name is { Length: > 0 } type ? type + "." : "") +
                  method.Name;
            return new ProfilerMarker(name);
        }
#endif

        internal bool IsEmpty => _enter is null && _update is null && _exit is null;

        public void Enter()
        {
#if ENABLE_PROFILER
            using (_enterMarker.Auto())
#endif
                _enter?.Invoke();
        }
        
        public void Update()
        {
#if ENABLE_PROFILER
            using (_updateMarker.Auto())
#endif
                _update?.Invoke();
        }

        public void Exit()
        {
#if ENABLE_PROFILER
            using (_exitMarker.Auto())
#endif
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