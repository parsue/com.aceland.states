using System;
using System.Threading.Tasks;
using AceLand.States.Core;
using AceLand.States.Exceptions;
using AceLand.TaskUtils;
using UnityEngine;

namespace AceLand.States
{
    public partial class StateMachine
    {
        public static Promise<IStateMachine> GetAsync(string id) => GetStateMachineAsync(id);
        public static Promise<IStateMachine> GetAsync<TEnum>(TEnum id) where TEnum : Enum => GetStateMachineAsync(id.ToString());

        public static IStateMachine Get(string id) =>
            StateMachines.TryGetMachine(id, out var stateMachine) ? stateMachine : null;
        public static IStateMachine Get<TEnum>(TEnum id) =>
            Get(id.ToString());

        private static async Task<IStateMachine> GetStateMachineAsync(string id)
        {
            var aliveToken = Promise.ApplicationAliveToken;
            var targetTime = Time.realtimeSinceStartup + Settings.GetterTimeout;

            while (!aliveToken.IsCancellationRequested && Time.realtimeSinceStartup < targetTime)
            {
                await Task.Yield();
                
                var arg = StateMachines.TryGetMachine(id, out var stateMachine);
                
                if (arg) return stateMachine;
            }

            throw new StateMachineNotFoundException($"State Machine [{id}] is not found");
        }
    }
}
