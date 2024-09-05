using System;

namespace AceLand.States.Core
{
    public interface IAnyState
    {
        string Name { get; }
        void StateEnter();
        void StateUpdate();
        void StateExit();
    }

    public interface IState : IAnyState
    {
        IState WithSubStateMachine(IStateMachine stateMachine);
        IState WithoutSubStateMachine(IStateMachine stateMachine);
        IState WithActions(Action enter = null, Action update = null, Action exit = null);
        IState WithoutActions(Action enter = null, Action update = null, Action exit = null);
    }

    public interface IIdleState : IAnyState
    {
        IIdleState WithActions(Action enter = null, Action exit = null);
        IIdleState WithoutActions(Action enter = null, Action exit = null);
    }
}