using System;

namespace AceLand.States.Core
{
    public interface IAnyState
    {
        string Name { get; }
        void StateEnter();
        void StateUpdate();
        void StateExit();
        bool CompareTo(IAnyState other);
    }

    public interface IState : IAnyState
    {
        IState InjectSubStateMachine(IStateMachine stateMachine);
        IState RemoveSubStateMachine(IStateMachine stateMachine);
        IState InjectActions(Action enter = null, Action update = null, Action exit = null);
        IState RemoveActions(Action enter = null, Action update = null, Action exit = null);
    }

    public interface IIdleState : IAnyState
    {
        IIdleState InjectActions(Action enter = null, Action exit = null);
        IIdleState RemoveActions(Action enter = null, Action exit = null);
    }
}