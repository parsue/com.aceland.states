using System;
using AceLand.Library.Optional;

namespace AceLand.States.Core
{
    public sealed class AnyState : IAnyState
    {
        private AnyState(string name)
        {
            Name = name;
        }
        
        #region Builder
        
        internal static IStateBuilder Builder() => new StateBuilder();

        internal interface IStateBuilder
        {
            IAnyState Build();
            IStateBuilder WithName(string name);
        }
        
        private class StateBuilder : IStateBuilder
        {
            private Option<string> _name = Option<string>.None();
            public IAnyState Build()
            {
                var name = _name.Reduce(Guid.NewGuid().ToString());
                return new AnyState(name);
            }
            
            public IStateBuilder WithName(string name)
            {
                _name = name.ToOption();
                return this;
            }
        }

        #endregion

        public string Name { get; }
        public void StateEnter()
        {
            //noop
        }

        public void StateUpdate()
        {
            //noop
        }

        public void StateExit()
        {
            //noop
        }

        public bool CompareTo(IAnyState other) =>
            other is not null && ReferenceEquals(this, other) && Name == other.Name;
    }
}