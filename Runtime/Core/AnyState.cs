using System;

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
        }
        
        private class StateBuilder : IStateBuilder
        {
            public IAnyState Build()
            {
                var name = Guid.NewGuid().ToString();
                return new AnyState(name);
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
    }
}