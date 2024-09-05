using System;
using System.Collections.Generic;
using AceLand.States.Core;

namespace AceLand.States
{
    public sealed class IdleState : IIdleState
    {
        private IdleState(string name, StateUpdater updater)
        {
            Name = name;
            _updater = updater;
        }
        
        #region Builder
        
        public static IStateBuilder Builder() => new StateBuilder();

        public interface IStateBuilder
        {
            IStateBuilder WithActions(Action enter = null, Action exit = null);
            IIdleState Build();
        }
        
        private class StateBuilder : IStateBuilder
        {
            private readonly List<StateAction> _actions = new();

            public IIdleState Build() 
            {
                var name = Guid.NewGuid().ToString();
                var updater = new StateUpdater(_actions);
                return new IdleState(name, updater);
            }

            public IStateBuilder WithActions(Action enter = null, Action exit = null)
            {
                _actions.Add(StatesHelper.CreateStateUpdater(enter: enter, exit: exit));
                return this;
            }
        }

        #endregion

        public string Name { get; }
        
        private readonly StateUpdater _updater;
        
        public void StateEnter() => _updater.Enter();
        public void StateUpdate() { }
        public void StateExit() => _updater.Exit();

        public IIdleState WithActions(Action enter = null, Action exit = null)
        {
            _updater.WithActions(enter: enter, exit: exit);
            return this;
        }

        public IIdleState WithoutActions(Action enter = null, Action exit = null)
        {
            _updater.WithoutActions(enter: enter, exit: exit);
            return this;
        }
    }
}