using System;
using System.Collections.Generic;
using AceLand.Library.Extensions;
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
            if (id.IsNullOrEmptyOrWhiteSpace()) return;
            if (!Machines.ContainsKey(id)) return;
            
            Machines.Remove(id);
        }
    }
}
