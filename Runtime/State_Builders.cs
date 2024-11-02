using System;
using System.Collections.Generic;
using AceLand.Library.Optional;
using AceLand.States.Core;

namespace AceLand.States
{
    public sealed partial class State
    {
        public static IStateNamesBuilders Builders() => new StateBuilders();

        public interface IStateNamesBuilders
        {
            IStateBuilders WithNames(params string[] name);
            IStateBuilders WithNames<TEnum>();
        }

        public interface IStateBuilders
        {
            IState[] Build();
        }
        
        private class StateBuilders : IStateNamesBuilders, IStateBuilders
        {
            private string[] _names;

            public IState[] Build()
            {
                var states = new List<IState>();
                foreach (var n in _names)
                {
                    var name = n.ToOption();
                    var updater = new StateUpdater();
                    var machines = new SubStateMachines();
                    states.Add(new State(name, updater, machines));
                }

                return states.ToArray();
            }
            
            public IStateBuilders WithNames(params string[] names)
            {
                _names = names;
                return this;
            }

            public IStateBuilders WithNames<TEnum>() =>
                WithNames(Enum.GetNames(typeof(TEnum)));
        }
    }
}