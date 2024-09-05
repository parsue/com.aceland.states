using System;
using AceLand.States.Core;

namespace AceLand.States.Mono
{
    public abstract class MonoIdleState : MonoStateBase, IIdleState
    {
        public new virtual void StateEnter() => Updater.Enter();
        public new virtual void StateExit() => Updater.Exit();
        
        public virtual IIdleState WithActions(Action enter = null, Action exit = null)
        {
            Updater.WithActions(enter: enter, exit: exit);
            return this;
        }

        public IIdleState WithoutActions(Action enter = null, Action exit = null)
        {
            Updater.WithoutActions(enter: enter, exit: exit);
            return this;
        }
    }
}