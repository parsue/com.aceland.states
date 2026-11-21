#nullable enable
using System;
using AceLand.States.Core;
#if ENABLE_PROFILER
using Unity.Profiling;
#endif

namespace AceLand.States
{
    public sealed partial class State :IState
    {
        private State(string? name, StateUpdater updater, SubStateMachines subStateMachines)
        {
            Name = name ?? Guid.NewGuid().ToString();
            _updater = updater;
            _subStateMachines = subStateMachines;
#if ENABLE_PROFILER
            // State-step markers so each state's Enter/Update/Exit shows as a distinct Profiler node,
            // nested under its owning machine via the call stack. Uses '.' separators (never '/').
            var prefix = "AceLand.States.State." + Name + ".";
            _enterMarker = new ProfilerMarker(prefix + "Enter");
            _updateMarker = new ProfilerMarker(prefix + "Update");
            _exitMarker = new ProfilerMarker(prefix + "Exit");
#endif
        }

        public string Name { get; }

        private readonly StateUpdater _updater;
        private readonly SubStateMachines _subStateMachines;

#if ENABLE_PROFILER
        private readonly ProfilerMarker _enterMarker;
        private readonly ProfilerMarker _updateMarker;
        private readonly ProfilerMarker _exitMarker;
#endif
        
        public void StateEnter()
        {
#if ENABLE_PROFILER
            using (_enterMarker.Auto())
#endif
            {
                _updater.Enter();
                _subStateMachines.Start();
            }
        }

        public void StateUpdate()
        {
#if ENABLE_PROFILER
            using (_updateMarker.Auto())
#endif
            {
                _updater.Update();
                _subStateMachines.Update();
            }
        }
        
        public void StateExit()
        {
#if ENABLE_PROFILER
            using (_exitMarker.Auto())
#endif
            {
                _updater.Exit();
                _subStateMachines.Stop();
            }
        }

        public IState InjectSubStateMachine(IStateMachine stateMachine)
        {
            _subStateMachines.InjectSubMachine(stateMachine);
            return this;
        }

        public IState RemoveSubStateMachine(IStateMachine stateMachine)
        {
            _subStateMachines.RemoveSubMachine(stateMachine);
            return this;
        }

        public IState InjectActions(Action enter = null, Action update = null, Action exit = null)
        {
            _updater.InjectActions(enter, update, exit);
            return this;
        }

        public IState RemoveActions(Action enter = null, Action update = null, Action exit = null)
        {
            _updater.RemoveActions(enter, update, exit);
            return this;
        }

        public bool CompareTo(IAnyState other) =>
            other is not null && ReferenceEquals(this, other) && Name == other.Name;
    }
}