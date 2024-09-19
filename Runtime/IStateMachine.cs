using AceLand.States.Core;

namespace AceLand.States
{
    public interface IAnyStateMachine
    {
        string Id { get; }
        IAnyState EntryState { get; }
        IAnyState CurrentState { get; }
        IAnyState ExitState { get; }
        IAnyState StartState { get; }
        IState GetState(string stateName);
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