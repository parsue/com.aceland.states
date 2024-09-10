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

        protected override void DisposeManagedResources()
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
        
        public virtual IStateMachine StartMachine()
        {
            ToEntryState();
            IsActive = true;
            if (PrintLogging)
                Debug.Log($"[{Id}] State Machine Started");
            
            return this;
        }

        public virtual void StopMachine()
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
            if (Disposed) return;

            CurrentState.StateUpdate();
        }

        private void ToEntryState()
        {
            if (Disposed) return;

            CurrentState?.StateExit();
            CurrentState = StartState;
            CurrentState.StateEnter();

            if (!PrintLogging) return;
            var msg = $"[{Id}] State Transition : Entry >> {CurrentState.Name}";
            Debug.Log(msg);
        }

        private void ToExitState()
        {
            if (Disposed) return;

            var stateName = CurrentState.Name;
            CurrentState.StateExit();
            CurrentState = ExitState;

            if (!PrintLogging) return;
            var msg = $"[{Id}] State Transition : {stateName} >> Exit";
            Debug.Log(msg);
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

        public void InjectAnyTransition(IState toState, Func<bool> argument, bool preventToSelf = true)
        {
            if (Disposed) return;
            
            var fromState = StatesHelper.AnyState;
            AnyTransitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
        }

        public void InjectAnyTransition(IIdleState toState, Func<bool> argument, bool preventToSelf = true)
        {
            if (Disposed) return;
            
            var fromState = StatesHelper.AnyState;
            AnyTransitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
        }

        public void InjectTransition(IState fromState, IState toState, Func<bool> argument, bool preventToSelf = false)
        {
            if (Disposed) return;

            Transitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
        }

        public void InjectTransition(IIdleState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = false)
        {
            if (Disposed) return;

            Transitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
        }

        public void InjectTransition(IState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = false)
        {
            if (Disposed) return;

            Transitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
        }

        public void InjectTransition(IIdleState fromState, IState toState, Func<bool> argument, bool preventToSelf = false)
        {
            if (Disposed) return;

            Transitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
        }
    }
}
