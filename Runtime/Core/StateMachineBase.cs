using System;
using System.Collections.Generic;
using AceLand.Library.Disposable;
using AceLand.Library.Extensions;
using AceLand.Library.Optional;
using AceLand.States.ProjectSetting;
using UnityEngine;

namespace AceLand.States.Core
{
    public abstract class StateMachineBase : DisposableObject, IStateMachine
    {
        private protected StateMachineBase(Option<string> id, IState[] states, IAnyState firstState,
            List<StateTransition> anyTransitions, List<StateTransition> transitions)
        {
            Id = id.Reduce(Guid.NewGuid().ToString());
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

        ~StateMachineBase() => Dispose(false);

        protected override void DisposeManagedResources()
        {
            if (!Id.IsNullOrEmptyOrWhiteSpace()) this.UnRegister();
            CurrentState = null;
            _transitions.Clear();
        }
        
        public string Id { get; }
        public bool IsActive { get; private set; }
        public IAnyState EntryState { get; }
        public IAnyState ExitState { get; }
        public IAnyState StartState { get; }
        public IAnyState CurrentState { get; private set; }
        
        private protected static StatesSettings Settings => StatesUtils.Settings;
        private static bool PrintLogging => Settings.PrintLogging;

        private IState[] States { get; }
        private readonly List<StateTransition> _anyTransitions = new();
        private readonly List<StateTransition> _transitions = new();
        
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
            if (Disposed || !IsActive) return;
            
            StateTransition();
            StateUpdate();
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

            throw new Exception($"Get State Error: name [{name}] not found");
        }
    }
}
