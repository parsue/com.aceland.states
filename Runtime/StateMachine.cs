using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AceLand.Library.Optional;
using AceLand.PlayerLoopHack;
using AceLand.States.Core;
using AceLand.TaskUtils;
using AceLand.TaskUtils.PromiseAwaiter;
using UnityEngine;

namespace AceLand.States
{
    public class StateMachine : StateMachineBase
    {
        private protected StateMachine(Option<string> id, IState[] states, IAnyState firstState,
            List<StateTransition> anyTransitions, List<StateTransition> transitions) :
            base(id, states, firstState, anyTransitions, transitions) { }
        
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

            public IStateMachineBuilder WithId<T>(T id) where T : Enum
            {
                _id = id.ToString();
                return this;
            }
        }

        #endregion

        #region Getter

        public static Promise<IStateMachine> Get(string id) => GetStateMachine(id);
        public static Promise<IStateMachine> Get<T>(T id) where T : Enum => GetStateMachine(id.ToString());

        private static async Task<IStateMachine> GetStateMachine(string id)
        {
            var aliveToken = TaskHelper.ApplicationAliveToken;
            var targetTime = Time.realtimeSinceStartup + Settings.getterTimeout;

            while (!aliveToken.IsCancellationRequested && Time.realtimeSinceStartup < targetTime)
            {
                await Task.Yield();
                
                var arg = StateMachines.TryGetMachine(id, out var stateMachine);
                
                if (arg) return stateMachine;
            }

            throw new Exception($"State Machine [{id}] is not found");
        }

        #endregion
        
        public override IStateMachine Start()
        {
            if (IsActive) return this;

            base.Start();
            return this;
        }

        public override void Stop()
        {
            if (!IsActive) return;
            
            base.Stop();
        }
    }
}
