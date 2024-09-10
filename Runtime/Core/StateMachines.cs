using System;
using System.Collections.Generic;
using AceLand.Library.Extensions;
using AceLand.States.ProjectSetting;
using UnityEngine;

namespace AceLand.States.Core
{
    internal static class StateMachines
    {
        private static StatesSettings Settings => StatesHelper.Settings;
        private static readonly Dictionary<string, IAnyStateMachine> _machines = new();

        internal static bool TryGetMachine(string id, out IStateMachine machine)
        {
            machine = null;
            if (!_machines.TryGetValue(id, out var anyMachine)) return false;
            if (anyMachine is not IStateMachine stateMachine) return false;

            machine = stateMachine;
            return true;
        }
        
        internal static void Register(this IAnyStateMachine machine)
        {
            var id = machine.Id;
            if (!_machines.TryAdd(id, machine))
                throw new Exception($"Add Machine Error: id [{id}] already exists");

            if (Settings.enableLogging)
            {
#if UNITY_EDITOR
                if (!Settings.loggingOnEditor) return;
#else
                if (!Settings.loggingOnBuild) return;
#endif
                Debug.Log($"State Machine [{id}] Registered");
            }
        }

        internal static void UnRegister(this IAnyStateMachine machine)
        {
            var id = machine.Id;
            if (id.IsNullOrEmptyOrWhiteSpace()) return;
            if (!_machines.ContainsKey(id)) return;
            
            _machines.Remove(id);

            if (Settings.enableLogging)
            {
#if UNITY_EDITOR
                if (!Settings.loggingOnEditor) return;
#else
                if (!Settings.loggingOnBuild) return;
#endif
                Debug.Log($"State Machine [{id}] UnRegistered");
            }
        }
    }
}
