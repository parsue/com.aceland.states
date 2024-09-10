using AceLand.Library.Disposable;
using AceLand.Library.Optional;
using System;

namespace AceLand.States.Core
{
    public abstract class StateBase : DisposableObject, IState
    {
        private protected StateBase(Option<string> name, StateUpdater updater, SubStateMachines subStateMachines)
        {
            Name = name.Reduce(Guid.NewGuid().ToString());
            _updater = updater;
            _subStateMachines = subStateMachines;
        }

        public string Name { get; }

        private readonly StateUpdater _updater;
        private readonly SubStateMachines _subStateMachines;
        
        public virtual void StateEnter()
        {
            _updater.Enter();
            _subStateMachines.StartMachine();
        }

        public virtual void StateUpdate()
        {
            _updater.Update();
        }
        
        public virtual void StateExit()
        {
            _updater.Exit();
            _subStateMachines.StopMachine();
        }

        public virtual IState WithSubStateMachine(IStateMachine stateMachine)
        {
            _subStateMachines.InjectSubMachine(stateMachine);
            return this;
        }

        public virtual IState WithoutSubStateMachine(IStateMachine stateMachine)
        {
            _subStateMachines.RemoveSubMachine(stateMachine);
            return this;
        }

        public virtual IState WithActions(Action enter = null, Action update = null, Action exit = null)
        {
            _updater.WithActions(enter, update, exit);
            return this;
        }

        public virtual IState WithoutActions(Action enter = null, Action update = null, Action exit = null)
        {
            _updater.WithoutActions(enter, update, exit);
            return this;
        }
    }
}