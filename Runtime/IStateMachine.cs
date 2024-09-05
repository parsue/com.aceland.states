using System;
using AceLand.States.Core;

namespace AceLand.States
{
    public interface IAnyStateMachine
    {
        string Id { get; }
        IAnyState EntryState { get; }
        IAnyState CurrentState { get; }
        IAnyState ExitState { get; }
        IState GetState(string stateName);
        void ToExitState();
        void ToEntryState();
    }

    public interface IStateMachine : IAnyStateMachine
    {
        bool IsActive { get; }
        void Dispose();
        IStateMachine StartEngine();
        void StopEngine();
        void Update();
        void WithAnyTransition(IState toState, Func<bool> argument, bool preventToSelf = true);
        void WithAnyTransition(IIdleState toState, Func<bool> argument, bool preventToSelf = true);
        void WithTransition(IState fromState, IState toState, Func<bool> argument, bool preventToSelf = false);
        void WithTransition(IIdleState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = false);
        void WithTransition(IState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = false);
        void WithTransition(IIdleState fromState, IState toState, Func<bool> argument, bool preventToSelf = false);
    }

    public interface IMonoStateMachine : IAnyStateMachine
    {
        void WithAnyTransition(IState toState, Func<bool> argument, bool preventToSelf = true);
        void WithAnyTransition(IIdleState toState, Func<bool> argument, bool preventToSelf = true);
        void WithTransition(IState fromState, IState toState, Func<bool> argument, bool preventToSelf = false);
        void WithTransition(IIdleState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = false);
        void WithTransition(IState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = false);
        void WithTransition(IIdleState fromState, IState toState, Func<bool> argument, bool preventToSelf = false);
    }
}