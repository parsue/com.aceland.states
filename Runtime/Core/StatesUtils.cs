using System;
using System.Collections.Generic;
using System.Reflection;
using AceLand.Library.Extensions;
using AceLand.States.ProjectSetting;
using UnityEngine;

namespace AceLand.States.Core
{
    internal static class StatesUtils
    {
        public static StatesSettings Settings
        {
            get
            {
                _settings ??= Resources.Load<StatesSettings>(nameof(StatesSettings));
                return _settings;
            }
            internal set => _settings = value;
        }
        
        private static StatesSettings _settings;
        private static bool PrintLogging => Settings.PrintLogging();
        
        public static IAnyState AnyState;
        public static IAnyState EntryState;
        public static IAnyState ExitState;
        
        public static StateAction CreateStateUpdater(Action enter = null, Action update = null, Action exit = null)
        {
            return new StateAction(enter, update, exit);
        }
        
        public static StateTransition CreateTransition(IAnyState fromState, IAnyState toState, Func<bool> argument, bool preventToSelf = true)
        {
            return new StateTransition(fromState, toState, argument, preventToSelf);
        }

        public static bool IsStateTransition(this IStateMachine machine,
            IEnumerable<StateTransition> anyTransitions, IEnumerable<StateTransition> transitions,
            out IAnyState nextState, out bool isAny)
        {
            var currentState = machine.CurrentState;
            foreach (var transition in anyTransitions)
            {
                if (!transition.IsNextState(currentState, out nextState)) continue;
                isAny = true;
                return true;
            }

            foreach (var transition in transitions)
            {
                if (!transition.IsNextState(currentState, out nextState)) continue;
                isAny = false;
                return true;
            }

            nextState = null;
            isAny = false;
            return false;
        }

        public static Func<bool> GetArgument(string funcName, ReadOnlySpan<MethodInfo> argMethodInfo)
        {
            if (funcName.IsNullOrEmptyOrWhiteSpace()) return null;
            if (argMethodInfo.Length == 0) return null;
            
            foreach (var info in argMethodInfo)
            {
                if (info.Name != funcName) continue;
                if (info.ReturnType != typeof(bool)) continue;
                if (info.GetParameters().Length > 0) continue;

                var func = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), info);
                return func;
            }

            throw new Exception($"Get Argument error: [{funcName}] is not found");
        }

        public static void PrintEntryState(this IStateMachine machine)
        {
            var entry = machine.EntryState;
            
            if (PrintLogging && !entry.Name.IsNullOrEmptyOrWhiteSpace())
                Debug.Log($"[{machine.Id}] State Entry : {entry.Name}");
        }

        public static void PrintStateTransitionLog(this IStateMachine machine, IAnyState next, bool isAny)
        {
            var current = machine.CurrentState;
            if (!PrintLogging || current.Name.IsNullOrEmptyOrWhiteSpace()) return;
            
            var fromName = GetStateName(current);
            var toName = GetStateName(next);

            if (!Settings.PrintLogging()) return;
            var msg = $"[{machine.Id}] State Transition{(isAny ? " [Any]" : "")} : {fromName} >> {toName}";
            Debug.Log(msg);
        }

        private static string GetStateName(IAnyState state)
        {
            return (state is IIdleState) switch
            {
                true => "Idle",
                false when state == AnyState => "Any",
                false when state == EntryState => "Entry",
                false when state == ExitState => "Exit",
                _ => state.Name,
            };
        }
        
    }
}