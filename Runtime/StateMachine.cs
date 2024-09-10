using System;
using System.Collections.Generic;
using AceLand.Library.Optional;
using AceLand.PlayerLoopHack;
using AceLand.States.Core;
using AceLand.States.Handler;

namespace AceLand.States
{
    public class StateMachine : StateMachineBase
    {
        private protected StateMachine(Option<string> id, IState[] states, IAnyState entryState,
            List<StateTransition> anyTransitions, List<StateTransition> transitions) :
            base(id, states, entryState, anyTransitions, transitions) { }

        public static StateMachineGetter Get(string id) => new(id);
        public static StateMachineSystemGetter GetSystem(string id) => new(id);
        
        #region Builder

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
            IStateMachineBuilder WithAnyTransition(IIdleState toState, Func<bool> argument, bool preventToSelf = true);
            IStateMachineBuilder WithTransition(IState fromState, IState toState, Func<bool> argument, bool preventToSelf = true);
            IStateMachineBuilder WithTransition(IIdleState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = true);
            IStateMachineBuilder WithTransition(IState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = true);
            IStateMachineBuilder WithTransition(IIdleState fromState, IState toState, Func<bool> argument, bool preventToSelf = true);
            IStateMachineBuilder WithId(string id);
            IStateMachine Build();
            IStateMachine BuildAsSystem(PlayerLoopType playerLoopType, int index = 0);
        }

        private class StateMachineBuilder : IStatesBuilder, IEntryTransitionBuilder, IStateMachineBuilder
        {
            private IState[] _states;
            private readonly List<StateTransition> _anyTransitions = new();
            private readonly List<StateTransition> _transitions = new();
            private IAnyState _entryState;
            private string _id;

            public IStateMachine Build() => 
                new StateMachine(_id.ToOption(), _states, _entryState, _anyTransitions, _transitions);
            
            public IStateMachine BuildAsSystem(PlayerLoopType playerLoopType, int index = 0) =>
                new StateMachineSystem(_id.ToOption(), _states, _entryState, _anyTransitions, _transitions, playerLoopType, index);

            public IEntryTransitionBuilder WithStates(IState[] states)
            {
                _states = states;
                return this;
            }
            
            public IStateMachineBuilder WithEntryTransition(IState toState)
            {
                _entryState = toState;
                return this;
            }

            public IStateMachineBuilder WithExitTransition(IState fromState, Func<bool> argument)
            {
                var toState = StatesHelper.ExitState;
                _anyTransitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, true));
                return this;
            }

            public IStateMachineBuilder WithAnyExitTransition(Func<bool> argument)
            {
                var fromState = StatesHelper.AnyState;
                var toState = StatesHelper.ExitState;
                _anyTransitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, true));
                return this;
            }
            
            public IStateMachineBuilder WithAnyTransition(IState toState, Func<bool> argument, bool preventToSelf = true)
            {
                var fromState = StatesHelper.AnyState;
                _anyTransitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
                return this;
            }

            public IStateMachineBuilder WithAnyTransition(IIdleState toState, Func<bool> argument, bool preventToSelf = true)
            {
                var fromState = StatesHelper.AnyState;
                _anyTransitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
                return this;
            }

            public IStateMachineBuilder WithTransition(IState fromState, IState toState, Func<bool> argument, bool preventToSelf = true)
            {
                _transitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
                return this;
            }

            public IStateMachineBuilder WithTransition(IIdleState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = true)
            {
                _transitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
                return this;
            }

            public IStateMachineBuilder WithTransition(IState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = true)
            {
                _transitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
                return this;
            }

            public IStateMachineBuilder WithTransition(IIdleState fromState, IState toState, Func<bool> argument, bool preventToSelf = true)
            {
                _transitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
                return this;
            }

            public IStateMachineBuilder WithId(string id)
            {
                _id = id;
                return this;
            }
        }

        #endregion
        
        public override IStateMachine StartMachine()
        {
            if (IsActive) return this;

            base.StartMachine();
            return this;
        }

        public override void StopMachine()
        {
            if (!IsActive) return;
            
            base.StopMachine();
        }
    }
}
