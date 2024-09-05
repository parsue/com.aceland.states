using System;
using System.Collections.Generic;
using AceLand.Library.Disposable;
using AceLand.Library.Extensions;
using AceLand.Library.Optional;
using AceLand.States.Core;
using AceLand.States.ProjectSetting;
using UnityEngine;

namespace AceLand.States
{
    public abstract class StateMachineBase : DisposableObject, IStateMachine
    {
        private protected StateMachineBase(Option<string> id, IState[] states, IAnyState entryState,
            List<StateTransition> anyTransitions, List<StateTransition> transitions)
        {
            Id = id.Reduce(Guid.NewGuid().ToString());
            States = states;
            EntryState = StatesHelper.EntryState;
            ExitState = StatesHelper.ExitState;
            AnyTransitions.AddRange(anyTransitions);
            Transitions.AddRange(transitions);
            CurrentState = entryState;
            StartState = entryState;

            this.Register();
        }

        protected override void BeforeDispose()
        {
            if (!Id.IsNullOrEmptyOrWhiteSpace()) this.UnRegister();
            CurrentState = null;
            Transitions.Clear();
        }
        
        public string Id { get; }
        public bool IsActive { get; protected set; }
        public IAnyState EntryState { get; }
        public IAnyState ExitState { get; }
        public IAnyState StartState { get; }
        public IAnyState CurrentState { get; private set; }
        
        private protected static StatesSettings Settings => StatesHelper.Settings;
        private protected static bool PrintLogging => Settings.PrintLogging();

        private IState[] States { get; }
        private protected readonly List<StateTransition> AnyTransitions = new();
        private protected readonly List<StateTransition> Transitions = new();
        
        public virtual IStateMachine StartEngine()
        {
            IsActive = true;
            if (PrintLogging)
                Debug.Log($"[{Id}] State Machine Started");
            
            return this;
        }

        public virtual void StopEngine()
        {
            IsActive = false;
            if (PrintLogging)
                Debug.Log($"[{Id}] State Machine Stopped");
        }
        
        public virtual void Update()
        {
            if (Disposed || !IsActive) return;
            
            StateTransition();
            StateUpdate();
        }

        private void StateTransition()
        {
            if (!this.IsStateTransition(AnyTransitions, Transitions,
                    out var nextState, out var isAny))
                return;
            
            CurrentState.StateExit();
            this.PrintStateTransitionLog(nextState, isAny);
            
            if (nextState == ExitState)
            {
                ToExitState();
                return;
            }
            CurrentState = nextState;
            CurrentState.StateEnter();
        }

        private void StateUpdate()
        {
            CurrentState.StateUpdate();
        }

        public void ToEntryState()
        {
            if (Disposed) return;

            CurrentState.StateExit();
            CurrentState = StartState;
            CurrentState.StateEnter();
            StartEngine();
            if (PrintLogging)
            {
                var msg = $"[{Id}] State Transition : Entry >> {CurrentState.Name}";
                Debug.Log(msg);
            }
        }

        public void ToExitState()
        {
            if (Disposed) return;

            CurrentState.StateExit();
            if (PrintLogging)
            {
                var msg = $"[{Id}] State Transition : {CurrentState.Name} >> Exit";
                Debug.Log(msg);
            }
            CurrentState = ExitState;
            StopEngine();
        }

        public IState GetState(string stateName)
        {
            if (Disposed) return null;

            foreach (var state in States)
            {
                if (state.Name != stateName) continue;
                return state;
            }

            throw new Exception($"Get State Error: name [{stateName}] not found");
        }

        public void WithAnyTransition(IState toState, Func<bool> argument, bool preventToSelf = true)
        {
            if (Disposed) return;
            
            var fromState = StatesHelper.AnyState;
            AnyTransitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
        }

        public void WithAnyTransition(IIdleState toState, Func<bool> argument, bool preventToSelf = true)
        {
            if (Disposed) return;
            
            var fromState = StatesHelper.AnyState;
            AnyTransitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
        }

        public void WithTransition(IState fromState, IState toState, Func<bool> argument, bool preventToSelf = false)
        {
            Transitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
        }

        public void WithTransition(IIdleState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = false)
        {
            Transitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
        }

        public void WithTransition(IState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = false)
        {
            Transitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
        }

        public void WithTransition(IIdleState fromState, IState toState, Func<bool> argument, bool preventToSelf = false)
        {
            Transitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
        }
    }
}