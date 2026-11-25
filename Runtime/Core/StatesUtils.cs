using System;
using System.Reflection;
using AceLand.States.ProjectSetting;
#if UNITY_6000_5_OR_NEWER
using Unity.Scripting.LifecycleManagement;
#endif
using UnityEngine;

namespace AceLand.States.Core
{
#if UNITY_6000_5_OR_NEWER
    internal static partial class StatesUtils
#else
    internal static class StatesUtils
#endif
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

#if UNITY_6000_5_OR_NEWER
        [AutoStaticsCleanup]
#endif
        private static StatesSettings _settings;

#if UNITY_6000_5_OR_NEWER
        [AutoStaticsCleanup]
#endif
        public static IAnyState AnyState;
#if UNITY_6000_5_OR_NEWER
        [AutoStaticsCleanup]
#endif
        public static IAnyState EntryState;
#if UNITY_6000_5_OR_NEWER
        [AutoStaticsCleanup]
#endif
        public static IAnyState ExitState;

#if !UNITY_6000_5_OR_NEWER
        // CoreCLR / no-domain-reload premise: on Unity versions without [AutoStaticsCleanup]
        // these statics survive between play sessions, so reset them explicitly on
        // (re)initialization to avoid stale state leaking across sessions.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void ClearStatic()
        {
            _settings = null;
            AnyState = null;
            EntryState = null;
            ExitState = null;
        }
#endif

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
            if (string.IsNullOrEmpty(funcName)) return null;
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
