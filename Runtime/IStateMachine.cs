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
    }

    public interface IStateMachine : IAnyStateMachine
    {
        bool IsActive { get; }
        void Dispose();
        IStateMachine StartMachine();
        void StopMachine();
        void Update();
        void InjectAnyTransition(IState toState, Func<bool> argument, bool preventToSelf = true);
        void InjectAnyTransition(IIdleState toState, Func<bool> argument, bool preventToSelf = true);
        void InjectTransition(IState fromState, IState toState, Func<bool> argument, bool preventToSelf = false);
        void InjectTransition(IIdleState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = false);
        void InjectTransition(IState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = false);
        void InjectTransition(IIdleState fromState, IState toState, Func<bool> argument, bool preventToSelf = false);
    }

    public interface IMonoStateMachine : IAnyStateMachine
    {
        void InjectAnyTransition(IState toState, Func<bool> argument, bool preventToSelf = true);
        void InjectAnyTransition(IIdleState toState, Func<bool> argument, bool preventToSelf = true);
        void InjectTransition(IState fromState, IState toState, Func<bool> argument, bool preventToSelf = false);
        void InjectTransition(IIdleState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = false);
        void InjectTransition(IState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = false);
        void InjectTransition(IIdleState fromState, IState toState, Func<bool> argument, bool preventToSelf = false);
    }
}