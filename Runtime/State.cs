using AceLand.Library.Disposable;
using AceLand.Library.Optional;
using System;
using AceLand.States.Core;

namespace AceLand.States
{
    public sealed partial class State : DisposableObject, IState
    {
        private State(Option<string> name, StateUpdater updater, SubStateMachines subStateMachines)
        {
            Name = name.Reduce(Guid.NewGuid().ToString());
            _updater = updater;
            _subStateMachines = subStateMachines;
        }

        ~State() => Dispose(false);

        public string Name { get; }

        private readonly StateUpdater _updater;
        private readonly SubStateMachines _subStateMachines;
        
        public void StateEnter()
        {
            _updater.Enter();
            _subStateMachines.StartMachine();
        }

        public void StateUpdate()
        {
            _updater.Update();
        }
        
        public void StateExit()
        {
            _updater.Exit();
            _subStateMachines.StopMachine();
        }

        public IState InjectSubStateMachine(IStateMachine stateMachine)
        {
            _subStateMachines.InjectSubMachine(stateMachine);
            return this;
        }

        public IState RemoveSubStateMachine(IStateMachine stateMachine)
        {
            _subStateMachines.RemoveSubMachine(stateMachine);
            return this;
        }

        public IState InjectActions(Action enter = null, Action update = null, Action exit = null)
        {
            _updater.InjectActions(enter, update, exit);
            return this;
        }

        public IState RemoveActions(Action enter = null, Action update = null, Action exit = null)
        {
            _updater.RemoveActions(enter, update, exit);
            return this;
        }

        public bool CompareTo(IAnyState other) =>
            other is not null && ReferenceEquals(this, other) && Name == other.Name;
    }
}