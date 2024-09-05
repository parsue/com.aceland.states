using System;
using System.Collections.Generic;
using System.Reflection;
using AceLand.Library.Attribute;
using AceLand.Library.Extensions;
using AceLand.PlayerLoopHack;
using AceLand.States.Core;
using AceLand.States.ProjectSetting;
using UnityEngine;
using UnityEngine.LowLevel;

namespace AceLand.States.Mono
{
    [RequireComponent(typeof(StateArgumentProvider))]
    [AddComponentMenu("AMVR/States/State Machine")]
    public sealed class MonoStateMachine : MonoBehaviour, IMonoStateMachine, IPlayerLoopSystem
    {
        [Header("State Machine")]
        [SerializeField] private string id;
        [SerializeField] private StateArgumentProvider argumentProvider;
        
        [Header("State Transition")]
        [SerializeField] private MonoStateBase entryState;
        [SerializeField] private MonoStateBase exitState;
        [SerializeField] private List<MonoStateTransition> stateTransitions;
        
        [Header("Player Loop System")]
        [SerializeField] private bool isSystem;
        [ConditionalShow("isSystem")]
        [SerializeField] private PlayerLoopType playerLoopType = PlayerLoopType.Update;
        
#if UNITY_EDITOR

        [InspectorButton]
        private void SetProvider() => TryGetComponent(out argumentProvider);
        
#endif
        
        private static PlayerLoopSystem _playerLoopSystem;
        private static StatesSettings Settings => StatesHelper.Settings;
        private static bool PrintLogging => Settings.PrintLogging();
        
        public string Id => id;
        public IAnyState EntryState => entryState;
        public IAnyState CurrentState { get; private set; }
        public IAnyState ExitState => exitState;

        private readonly List<StateTransition> _anyTransitions = new();
        private readonly List<StateTransition> _transitions = new();
        private bool _isStarted;

        private void OnValidate()
        {
            foreach (var transition in stateTransitions)
            {
                var from = (transition.IsAny) switch
                {
                    true => "[ Any ]",
                    false when transition.From == null => "??",
                    false => transition.From.Name,
                };
                var to = (transition.To == null) ? "??": transition.To.Name;
                var func = transition.FuncName.IsNullOrEmptyOrWhiteSpace() ? "??" : transition.FuncName;
                var prevent = $"-{(transition.PreventToSelf ? "\u00d7" : "○")}→";
                transition.name = $"{from} → {to} | {func} | {prevent}";
            }
        }

        private void Awake()
        {
            InitialTransitions();
            this.Register();
        }

        private void OnEnable()
        {
            StartSystem();
        }

        private void OnDisable()
        {
            StopSystem();
        }

        public void SystemUpdate()
        {
            MachineUpdate();
        }

        private void Update()
        {
            if (isSystem) return;
            MachineUpdate();
        }

        private void MachineUpdate()
        {
            StateTransition();
            StateUpdate();
        }

        private void StateTransition()
        {
            if (CurrentState == null)
            {
                this.PrintEntryState();
                CurrentState = entryState;
                CurrentState.StateEnter();
            }

            if (!this.IsStateTransition(_anyTransitions, _transitions,
                    out var nextState, out var isAny))
                return;

            CurrentState.StateExit();
            this.PrintStateTransitionLog(nextState, isAny);
            
            if (nextState == ExitState)
            {
                ToExitState();
                return;
            }
            CurrentState = nextState;
            CurrentState.StateEnter();
        }

        private void StateUpdate()
        {
            CurrentState.StateUpdate();
        }

        public void ToExitState()
        {
            if (!enabled) return;

            ExitState.StateEnter();
            CurrentState = EntryState;
            enabled = false;
        }

        public void ToEntryState()
        {
            if (enabled) return;
            
            CurrentState = EntryState;
            CurrentState.StateEnter();
            enabled = true;
        }

        private void InitialTransitions()
        {
            if (argumentProvider == null) return;
            
            var type = argumentProvider.GetType();
            ReadOnlySpan<MethodInfo> argSpan = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
            
            foreach (var transition in stateTransitions)
            {
                var t = StatesHelper.CreateTransition(
                    transition.From,
                    transition.To,
                    StatesHelper.GetArgument(transition.FuncName, argSpan),
                    transition.PreventToSelf);
                
                if (transition.IsAny) _anyTransitions.Add(t);
                else _transitions.Add(t);
            }
        }

        private void StartSystem()
        {
            if (!isSystem) return;
            if (_isStarted) return;
            
            _playerLoopSystem = this.CreatePlayerLoopSystem();
            _playerLoopSystem.InsertSystem(playerLoopType);
            _isStarted = true;
        }

        private void StopSystem()
        {
            _playerLoopSystem.RemoveSystem(playerLoopType);
            _isStarted = false;
        }
        
        public IState GetState(string stateName)
        {
            foreach (var transition in _transitions)
            {
                if (!transition.TryGetState(stateName, out var anyState)) continue;
                if (anyState is not IState state) continue;
                return state;
            }

            throw new Exception($"Get State Error: name [{stateName}] not found");
        }

        public void WithAnyTransition(IState toState, Func<bool> argument, bool preventToSelf = true)
        {
            var fromState = StatesHelper.AnyState;
            _anyTransitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
        }

        public void WithAnyTransition(IIdleState toState, Func<bool> argument, bool preventToSelf = true)
        {
            var fromState = StatesHelper.AnyState;
            _anyTransitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
        }

        public void WithTransition(IState fromState, IState toState, Func<bool> argument, bool preventToSelf = false)
        {
            _transitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
        }

        public void WithTransition(IIdleState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = false)
        {
            _transitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
        }

        public void WithTransition(IState fromState, IIdleState toState, Func<bool> argument, bool preventToSelf = false)
        {
            _transitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
        }

        public void WithTransition(IIdleState fromState, IState toState, Func<bool> argument, bool preventToSelf = false)
        {
            _transitions.Add(StatesHelper.CreateTransition(fromState, toState, argument, preventToSelf));
        }
    }
}