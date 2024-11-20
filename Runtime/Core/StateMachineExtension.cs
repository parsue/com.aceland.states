using AceLand.Library.Extensions;
using UnityEngine;

namespace AceLand.States.Core
{
    internal static class StateMachineExtension
    {
        private static bool PrintLogging => StatesUtils.Settings.PrintLogging;

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
            
            var fromName = StatesUtils.GetStateName(current);
            var toName = StatesUtils.GetStateName(next);

            if (!PrintLogging) return;
            var msg = $"[{machine.Id}] State Transition{(isAny ? " [Any]" : "")} : {fromName} >> {toName}";
            Debug.Log(msg);
        }
    }
}