using System;
using AceLand.States.Core;

namespace AceLand.States
{
    public interface IAnyStateMachine : IComparable, IComparable<IAnyStateMachine>, IEquatable<IAnyStateMachine>
    {
        string Id { get; }
        IAnyState EntryState { get; }
        IAnyState CurrentState { get; }
        IAnyState ExitState { get; }
        IAnyState StartState { get; }
        IState GetState(string stateName);
        IState GetState<T>(T stateName) where T : Enum;
    }

    public interface IStateMachine : IAnyStateMachine
    {
        bool IsActive { get; }
        void Dispose();
        IStateMachine Start();
        void Stop();
        void Update();
    }
}
