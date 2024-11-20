using System;
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

        public static string GetStateName(IAnyState state)
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