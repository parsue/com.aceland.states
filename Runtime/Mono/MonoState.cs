using System;
using AceLand.States.Core;

namespace AceLand.States.Mono
{
    public abstract class MonoState : MonoStateBase, IState
    {
        public new virtual void StateEnter()
        {
            Updater.Enter();
            SubStateMachines.ToEntry();
        }

        public new virtual void StateUpdate()
        {
            Updater.Update();
        }
        
        public new virtual void StateExit()
        {
            Updater.Exit();
            SubStateMachines.ToExit();
        }

        public virtual IState WithSubStateMachine(IStateMachine stateMachine)
        {
            SubStateMachines.WithSubMachine(stateMachine);
            return this;
        }

        public virtual IState WithoutSubStateMachine(IStateMachine stateMachine)
        {
            SubStateMachines.WithOutSubMachine(stateMachine);
            return this;
        }

        public virtual IState WithActions(Action enter = null, Action update = null, Action exit = null)
        {
            Updater.WithActions(enter, update, exit);
            return this;
        }

        public IState WithoutActions(Action enter = null, Action update = null, Action exit = null)
        {
            Updater.WithoutActions(enter, update, exit);
            return this;
        }
    }
}