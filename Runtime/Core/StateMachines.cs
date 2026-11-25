using System;
using System.Collections.Generic;
using AceLand.States.ProjectSetting;

namespace AceLand.States.Core
{
    internal static class StateMachines
    {
        private static StatesSettings Settings => StatesUtils.Settings;
        private static readonly Dictionary<string, IAnyStateMachine> Machines = new();

        internal static bool TryGetMachine(string id, out IStateMachine machine)
        {
            machine = null;
            if (!Machines.TryGetValue(id, out var anyMachine)) return false;
            if (anyMachine is not IStateMachine stateMachine) return false;

            machine = stateMachine;
            return true;
        }
        
        internal static void Register(this IAnyStateMachine machine)
        {
            var id = machine.Id;
            if (!Machines.TryAdd(id, machine))
                throw new Exception($"Add Machine Error: id [{id}] already exists");
        }

        internal static void UnRegister(this IAnyStateMachine machine)
        {
            var id = machine.Id;
            if (string.IsNullOrEmpty(id)) return;
            if (!Machines.ContainsKey(id)) return;
            
            Machines.Remove(id);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Editor-only: dispose and clear every registered machine.
        /// No-domain-reload play mode keeps the static <see cref="Machines"/> dictionary alive
        /// between play sessions, so it must be cleared on (re)initialization to avoid stale
        /// machines leaking across sessions. Player builds get a fresh static state per process
        /// launch, so this is not compiled there.
        /// </summary>
        internal static void ClearAll()
        {
            if (Machines.Count == 0) return;

            // Snapshot and clear first so Dispose -> UnRegister becomes a no-op
            // and we avoid mutating the dictionary while iterating.
            var machines = new List<IAnyStateMachine>(Machines.Values);
            Machines.Clear();

            foreach (var machine in machines)
            {
                if (machine is IStateMachine stateMachine)
                    stateMachine.Dispose();
            }
        }
#endif
    }
}
