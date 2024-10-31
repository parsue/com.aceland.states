using System;
using System.Collections.Generic;
using AceLand.Library.Optional;
using AceLand.PlayerLoopHack;
using AceLand.States.Core;

namespace AceLand.States
{
    public partial class StateMachine
    {
        public static IStatesBuilder Builder() => new StateMachineBuilder();

        public interface IStatesBuilder
        {
            IEntryTransitionBuilder WithStates(IState[] states);
        }
        
        public interface IEntryTransitionBuilder
        {
            IStateMachineBuilder WithEntryTransition(IState toState);
        }

        public interface IStateMachineBuilder
        {
            IStateMachineBuilder WithExitTransition(IState fromState, Func<bool> argument);
            IStateMachineBuilder WithAnyExitTransition(Func<bool> argument);
            IStateMachineBuilder WithAnyTransition(IState toState, Func<bool> argument, bool preventToSelf = true);
            IStateMachineBuilder WithTransition(IState fromState, IState toState, Func<bool> argument, bool preventToSelf = true);
            IStateMachineBuilder WithId(string id);
            IStateMachineBuilder WithId<T>(T id) where T : Enum;
            IStateMachine Build();
            IStateMachine BuildAsSystem(PlayerLoopType playerLoopType, int index = 0);
        }

        private class StateMachineBuilder : IStatesBuilder, IEntryTransitionBuilder, IStateMachineBuilder
        {
            private IState[] _states;
            private readonly List<StateTransition> _anyTransitions = new();
            private readonly List<StateTransition> _transitions = new();
            private IAnyState _firstState;
            private string _id;

            public IStateMachine Build() => 
                new StateMachine(_id.ToOption(), _states, _firstState, _anyTransitions, _transitions);
            
            public IStateMachine BuildAsSystem(PlayerLoopType playerLoopType, int index = 0) =>
                new StateMachineSystem(_id.ToOption(), _states, _firstState, _anyTransitions, _transitions, playerLoopType, index);

            public IEntryTransitionBuilder WithStates(IState[] states)
            {
                _states = states;
                return this;
            }
            
            public IStateMachineBuilder WithEntryTransition(IState toState)
            {
                _firstState = toState;
                return this;
            }

            public IStateMachineBuilder WithExitTransition(IState fromState, Func<bool> argument)
            {
                var toState = StatesUtils.ExitState;
                _anyTransitions.Add(StatesUtils.CreateTransition(fromState, toState, argument, true));
                return this;
            }

            public IStateMachineBuilder WithAnyExitTransition(Func<bool> argument)
            {
                var fromState = StatesUtils.AnyState;
                var toState = StatesUtils.ExitState;
                _anyTransitions.Add(StatesUtils.CreateTransition(fromState, toState, argument, true));
                return this;
            }
            
            public IStateMachineBuilder WithAnyTransition(IState toState, Func<bool> argument, bool preventToSelf = true)
            {
                var fromState = StatesUtils.AnyState;
                _anyTransitions.Add(StatesUtils.CreateTransition(fromState, toState, argument, preventToSelf));
                return this;
            }

            public IStateMachineBuilder WithAnyTransition(IIdleState toState, Func<bool> argument, bool preventToSelf = true)
            {
                var fromState = StatesUtils.AnyState;
                _anyTransitions.Add(StatesUtils.CreateTransition(fromState, toState, argument, preventToSelf));
                return this;
            }

            public IStateMachineBuilder WithTransition(IState fromState, IState toState, Func<bool> argument, bool preventToSelf = true)
            {
                _transitions.Add(StatesUtils.CreateTransition(fromState, toState, argument, preventToSelf));
                return this;
            }

            public IStateMachineBuilder WithTransition(IIdleState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = true)
            {
                _transitions.Add(StatesUtils.CreateTransition(fromState, toState, argument, preventToSelf));
                return this;
            }

            public IStateMachineBuilder WithTransition(IState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = true)
            {
                _transitions.Add(StatesUtils.CreateTransition(fromState, toState, argument, preventToSelf));
                return this;
            }

            public IStateMachineBuilder WithTransition(IIdleState fromState, IState toState, Func<bool> argument, bool preventToSelf = true)
            {
                _transitions.Add(StatesUtils.CreateTransition(fromState, toState, argument, preventToSelf));
                return this;
            }

            public IStateMachineBuilder WithId(string id)
            {
                _id = id;
                return this;
            }

            public IStateMachineBuilder WithId<T>(T id) where T : Enum
            {
                _id = id.ToString();
                return this;
            }
        }
    }
}
