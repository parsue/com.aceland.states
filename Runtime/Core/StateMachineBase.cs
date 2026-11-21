using System;
using System.Collections.Generic;
using AceLand.States.Exceptions;
using AceLand.States.ProjectSetting;
using UnityEngine;
#if ENABLE_PROFILER
using Unity.Profiling;
#endif

namespace AceLand.States.Core
{
    public abstract partial class StateMachineBase : IStateMachine
    {
        private protected StateMachineBase(string id, IState[] states, IAnyState firstState,
            List<StateTransition> anyTransitions, List<StateTransition> transitions)
        {
            Id = id ?? Guid.NewGuid().ToString();
#if ENABLE_PROFILER
            // Per-machine marker keyed by Id so every state machine shows as a distinct Profiler
            // node, instead of collapsing under a single Lifecycle Frame call site
            // (AceLand.Lifecycle.Frame.StateMachineSystem.Start) shared by all machines.
            _updateMarker = new ProfilerMarker(ProfilerPrefix + Id);
#endif
            States = states;
            EntryState = StatesUtils.EntryState;
            ExitState = StatesUtils.ExitState;
            StartState = firstState;
            _transitions.Add(StatesUtils.CreateTransition(EntryState, StartState, () => IsActive));
            
            _anyTransitions.AddRange(anyTransitions);
            _transitions.AddRange(transitions);
            CurrentState = EntryState;
            
            this.Register();
        }

        ~StateMachineBase() => Dispose();

        public virtual void Dispose()
        {
            if (Disposed) return;
            if (!string.IsNullOrEmpty(Id)) this.UnRegister();
            
            CurrentState = null;
            _transitions.Clear();
            Disposed = true;
        }
        
        public string Id { get; }
        public bool IsActive { get; private set; }
        public bool Disposed { get; private set; }
        public IAnyState EntryState { get; }
        public IAnyState ExitState { get; }
        public IAnyState StartState { get; }
        public IAnyState CurrentState { get; private set; }
        
        private protected static StatesSettings Settings => StatesUtils.Settings;
        private static bool PrintLogging => Settings.PrintLogging;

        private IState[] States { get; }
        private readonly List<StateTransition> _anyTransitions = new();
        private readonly List<StateTransition> _transitions = new();

#if ENABLE_PROFILER
        // Profiler marker name prefix; uses '.' separators (never '/') to form a single Profiler node.
        private const string ProfilerPrefix = "AceLand.States.Update.";
        private readonly ProfilerMarker _updateMarker;
#endif
        
        public virtual IStateMachine Start()
        {
            if (PrintLogging)
                Debug.Log($"[{Id}] State Machine Started");
            
            ToEntryState();
            IsActive = true;
            
            return this;
        }

        public virtual void Stop()
        {
            IsActive = false;
            ToExitState();
            
            if (PrintLogging)
                Debug.Log($"[{Id}] State Machine Stopped");
        }
        
        public virtual void Update()
        {
#if ENABLE_PROFILER
            using (_updateMarker.Auto())
#endif
            {
                StateTransition();
                StateUpdate();
            }
        }

        public IState GetState(string stateName) => GetStateByName(stateName);
        public IState GetState<T>(T stateName) where T : Enum => GetStateByName(stateName.ToString());

        private protected void StateTransition()
        {
            if (!this.IsStateTransition(_anyTransitions, _transitions,
                    out var nextState, out var isAny))
                return;
            
            CurrentState?.StateExit();
            this.PrintStateTransitionLog(nextState, isAny);
            
            CurrentState = nextState;
            CurrentState?.StateEnter();
        }

        private protected void StateUpdate()
        {
            CurrentState?.StateUpdate();
        }

        private void ToEntryState()
        {
            CurrentState?.StateExit();
            CurrentState = EntryState;
            
            if (PrintLogging)
                Debug.Log($"[{Id}] State Transition : Entry");
        }

        private void ToExitState()
        {
            CurrentState?.StateExit();
            CurrentState = ExitState;

            if (PrintLogging)
                Debug.Log($"[{Id}] State Transition : Exit");
        }

        private IState GetStateByName(string name)
        {
            if (Disposed) return null;

            foreach (var state in States)
            {
                if (state.Name != name) continue;
                return state;
            }

            throw new StateNotFoundException($"Get State Error: name [{name}] not found");
        }
    }
}
