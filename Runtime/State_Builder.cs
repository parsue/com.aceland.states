using System;
using System.Collections.Generic;
using AceLand.Library.Optional;
using AceLand.States.Core;

namespace AceLand.States
{
    public sealed partial class State
    {
        public static IStateBuilder Builder() => new StateBuilder();

        public interface IStateBuilder
        {
            IStateBuilder WithName(string name);
            IStateBuilder WithName<T>(T name) where T : Enum;
            IStateBuilder WithActions(Action enter = null, Action update = null, Action exit = null);
            IStateBuilder WithMachines(params IStateMachine[] machines);
            IState Build();
        }
        
        private class StateBuilder : IStateBuilder
        {
            private string _name = string.Empty;
            private readonly List<StateAction> _actions = new();
            private readonly List<IStateMachine> _machines = new();

            public IState Build()
            {
                var updater = new StateUpdater(_actions);
                var subStateMachines = new SubStateMachines(_machines);
                return new State(_name.ToOption(), updater, subStateMachines);
            }
            
            public IStateBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public IStateBuilder WithName<T>(T name) where T : Enum
            {
                _name = name.ToString();
                return this;
            }

            public IStateBuilder WithActions(Action enter = null, Action update = null, Action exit = null)
            {
                _actions.Add(StatesUtils.CreateStateUpdater(enter, update, exit));
                return this;
            }

            public IStateBuilder WithMachines(params IStateMachine[] machines)
            {
                _machines.AddRange(machines);
                return this;
            }
        }
    }
}